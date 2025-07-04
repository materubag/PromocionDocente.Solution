using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Infrastructure.Utils;
using PromocionDocente.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulacionesController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public PostulacionesController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/Postulaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postulacione>>> GetPostulaciones()
        {
            return await _context.Postulaciones.ToListAsync();
        }

        // GET: api/Postulaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postulacione>> GetPostulacione(int id)
        {
            var postulacione = await _context.Postulaciones.FindAsync(id);

            if (postulacione == null)
            {
                return NotFound();
            }

            return postulacione;
        }

        // PUT: api/Postulaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostulacione(int id, Postulacione postulacione)
        {
            if (id != postulacione.IdPos)
            {
                return BadRequest();
            }

            _context.Entry(postulacione).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostulacioneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Postulaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostulacione(int id)
        {
            var postulacione = await _context.Postulaciones.FindAsync(id);
            if (postulacione == null)
            {
                return NotFound();
            }

            _context.Postulaciones.Remove(postulacione);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostulacioneExists(int id)
        {
            return _context.Postulaciones.Any(e => e.IdPos == id);
        }


        // POST: api/Postulaciones
        [HttpPost]
        public async Task<IActionResult> PostPostulacione([FromBody] string input)
        {
            // Obtener la fecha actual y el usuario
            var fechaActual = DateTime.UtcNow;
            string cedula = input.Trim();
            string connectionString = _context.Database.GetConnectionString();
            string idCat = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // 1. Obtener la última categoría del historial del docente
                string selectQuery = @"
            SELECT TOP 1 ID_CAT
            FROM HISTORIAL_DOCENTE
            WHERE CED_DOC = @CedDoc
            ORDER BY FEC_INI DESC;";

                using (SqlCommand selectCmd = new SqlCommand(selectQuery, connection))
                {
                    selectCmd.Parameters.AddWithValue("@CedDoc", cedula);
                    var result = await selectCmd.ExecuteScalarAsync();

                    if (result == null)
                        return NotFound("No se encontró historial para el docente.");

                    idCat = result.ToString();
                }

                // 2. Verificar si ya existe una postulación en REVISION para esta cédula y categoría
                string checkExistingQuery = @"
            SELECT COUNT(*)
            FROM POSTULACIONES
            WHERE CED_DOC = @CedDoc 
            AND ID_CAT = @IdCat 
            AND (EST_POS IS NULL OR EST_POS = 'REVISION');";

                using (SqlCommand checkCmd = new SqlCommand(checkExistingQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@CedDoc", cedula);
                    checkCmd.Parameters.AddWithValue("@IdCat", idCat);

                    int existingCount = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());

                    if (existingCount > 0)
                    {
                        return BadRequest("Ya existe una postulación en revisión para este docente y categoría.");
                    }
                }

                // 3. Insertar la nueva postulación con estado inicial "REVISION"
                string insertQuery = @"
            INSERT INTO POSTULACIONES (CED_DOC, ID_CAT, FEC_POS, EST_POS)
            OUTPUT INSERTED.ID_POS
            VALUES (@CedDoc, @IdCat, @FecPos, @EstPos);";

                int newPostulacionId;
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@CedDoc", cedula);
                    insertCmd.Parameters.AddWithValue("@IdCat", idCat);
                    insertCmd.Parameters.AddWithValue("@FecPos", fechaActual);
                    insertCmd.Parameters.AddWithValue("@EstPos", "REVISION");

                    // Obtener el ID de la postulación insertada
                    newPostulacionId = Convert.ToInt32(await insertCmd.ExecuteScalarAsync());
                }

                // 4. Devolver información sobre la postulación creada
                return Ok(new
                {
                    message = "Postulación creada exitosamente",
                    id = newPostulacionId,
                    cedula = cedula,
                    categoria = idCat,
                    fecha = fechaActual,
                    estado = "REVISION"
                });
            }
        }

        // PUT: api/Postulaciones/evaluar
        [HttpPut("evaluar")]
        public async Task<IActionResult> AprobarRechazarPostulacionPorCedula(PostulacionAprobacionDto dto)
        {
            // Fecha actual para el registro (usando el formato proporcionado)
            var fechaActual = DateOnly.FromDateTime(DateTime.UtcNow); 

            // Usar el usuario que viene en el JSON
            var usuarioActual = dto.Usuario;

            // Validar que el usuario no esté vacío
            if (string.IsNullOrWhiteSpace(usuarioActual))
            {
                return BadRequest("Se requiere un nombre de usuario válido");
            }

            // 1. Buscar la última postulación del docente
            var ultimaPostulacion = await _context.Postulaciones
                .Where(p => p.CedDoc == dto.CedulaDocente)
                .OrderByDescending(p => p.FecPos)
                .FirstOrDefaultAsync();

            if (ultimaPostulacion == null)
            {
                return NotFound($"No se encontró ninguna postulación para el docente con cédula {dto.CedulaDocente}");
            }

            // 2. Buscar la última entrada en el historial del docente
            var ultimoHistorial = await _context.HistorialDocentes
                .Where(h => h.CedDoc == dto.CedulaDocente)
                .OrderByDescending(h => h.FecIni)
                .FirstOrDefaultAsync();

            if (ultimoHistorial == null)
            {
                return BadRequest("No se encontró historial para el docente");
            }

            var ultimaFechaHistorial = ultimoHistorial.FecIni;
            var categoriaActualId = ultimoHistorial.IdCat;

            // 3. Verificar si hay una promoción reciente (dentro de los últimos 30 días)
            var promocionesRecientes = await _context.HistorialDocentes
                .Where(h => h.CedDoc == dto.CedulaDocente &&
                            h.FecIni >= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)))
                .CountAsync();

            if (promocionesRecientes > 0)
            {
                return BadRequest("No se puede aprobar la postulación porque hay una promoción reciente (menos de 30 días)");
            }

            // 4. Validar según el estado solicitado
            bool cumpleRequisitos = false;
            string mensaje = "";

            if (dto.Estado == "APROBADO")
            {
                // 4.1 Contar si hay documentos existentes desde la última promoción
                var cantidadObras = await _context.Obras
                    .Where(o => o.CedDoc == dto.CedulaDocente && o.FechaPublicacion >= ultimaFechaHistorial)
                    .CountAsync();

                var cantidadInvestigaciones = await _context.Investigaciones
                    .Where(i => i.CedDoc == dto.CedulaDocente && i.FechaInicio >= ultimaFechaHistorial)
                    .CountAsync();

                var cantidadCursos = await _context.CursosCapacitacions
                    .Where(c => c.CedDoc == dto.CedulaDocente && c.FechaCurso >= ultimaFechaHistorial)
                    .CountAsync();

                var cantidadEvaluaciones = await _context.Evaluaciones
                    .Where(e => e.CedDoc == dto.CedulaDocente && e.FechaEvaluacion >= ultimaFechaHistorial)
                    .CountAsync();

                // 4.2 Verificar que existan documentos en cada categoría requerida
                bool existenDocumentosMinimos = cantidadObras >= 1 &&
                                               cantidadInvestigaciones >= 1 &&
                                               cantidadCursos >= 1 &&
                                               cantidadEvaluaciones >= 1;

                if (!existenDocumentosMinimos)
                {
                    mensaje = "No se puede aprobar la postulación porque no hay suficientes documentos en cada categoría. " +
                             $"Obras: {cantidadObras}/1, Investigaciones: {cantidadInvestigaciones}/1, " +
                             $"Cursos: {cantidadCursos}/1, Evaluaciones: {cantidadEvaluaciones}/1";
                    return BadRequest(mensaje);
                }

                // 4.3 Verificar que todos los documentos existentes estén aprobados
                bool todasObrasAprobadas = await _context.Obras
                    .Where(o => o.CedDoc == dto.CedulaDocente && o.FechaPublicacion >= ultimaFechaHistorial)
                    .AllAsync(o => o.Estado == "APROBADO");

                bool todasInvestigacionesAprobadas = await _context.Investigaciones
                    .Where(i => i.CedDoc == dto.CedulaDocente && i.FechaInicio >= ultimaFechaHistorial)
                    .AllAsync(i => i.Estado == "APROBADO");

                bool todosCursosAprobados = await _context.CursosCapacitacions
                    .Where(c => c.CedDoc == dto.CedulaDocente && c.FechaCurso >= ultimaFechaHistorial)
                    .AllAsync(c => c.Estado == "APROBADO");

                bool todasEvaluacionesAprobadas = await _context.Evaluaciones
                    .Where(e => e.CedDoc == dto.CedulaDocente && e.FechaEvaluacion >= ultimaFechaHistorial)
                    .AllAsync(e => e.Estado == "APROBADO");

                cumpleRequisitos = todasObrasAprobadas && todasInvestigacionesAprobadas &&
                                   todosCursosAprobados && todasEvaluacionesAprobadas;

                if (!cumpleRequisitos)
                {
                    mensaje = "No se puede aprobar la postulación porque no todos los requisitos están aprobados";
                }
            }
            else if (dto.Estado == "RECHAZADO")
            {
                // Verificar que todas las obras estén revisadas y al menos una rechazada
                var obrasRevisadas = await _context.Obras
                    .Where(o => o.CedDoc == dto.CedulaDocente && o.FechaPublicacion >= ultimaFechaHistorial)
                    .AllAsync(o => o.Estado == "APROBADO" || o.Estado == "RECHAZADO");

                var algunaObraRechazada = await _context.Obras
                    .Where(o => o.CedDoc == dto.CedulaDocente && o.FechaPublicacion >= ultimaFechaHistorial)
                    .AnyAsync(o => o.Estado == "RECHAZADO");

                // Verificar que todas las investigaciones estén revisadas y al menos una rechazada
                var investigacionesRevisadas = await _context.Investigaciones
                    .Where(i => i.CedDoc == dto.CedulaDocente && i.FechaInicio >= ultimaFechaHistorial)
                    .AllAsync(i => i.Estado == "APROBADO" || i.Estado == "RECHAZADO");

                var algunaInvestigacionRechazada = await _context.Investigaciones
                    .Where(i => i.CedDoc == dto.CedulaDocente && i.FechaInicio >= ultimaFechaHistorial)
                    .AnyAsync(i => i.Estado == "RECHAZADO");

                // Verificar que todos los cursos estén revisados y al menos uno rechazado
                var cursosRevisados = await _context.CursosCapacitacions
                    .Where(c => c.CedDoc == dto.CedulaDocente && c.FechaCurso >= ultimaFechaHistorial)
                    .AllAsync(c => c.Estado == "APROBADO" || c.Estado == "RECHAZADO");

                var algunCursoRechazado = await _context.CursosCapacitacions
                    .Where(c => c.CedDoc == dto.CedulaDocente && c.FechaCurso >= ultimaFechaHistorial)
                    .AnyAsync(c => c.Estado == "RECHAZADO");

                // Verificar que todas las evaluaciones estén revisadas y al menos una rechazada
                var evaluacionesRevisadas = await _context.Evaluaciones
                    .Where(e => e.CedDoc == dto.CedulaDocente && e.FechaEvaluacion >= ultimaFechaHistorial)
                    .AllAsync(e => e.Estado == "APROBADO" || e.Estado == "RECHAZADO");

                var algunaEvaluacionRechazada = await _context.Evaluaciones
                    .Where(e => e.CedDoc == dto.CedulaDocente && e.FechaEvaluacion >= ultimaFechaHistorial)
                    .AnyAsync(e => e.Estado == "RECHAZADO");

                // Todo debe estar revisado y al menos un elemento debe estar rechazado
                bool todosRevisados = obrasRevisadas && investigacionesRevisadas &&
                                      cursosRevisados && evaluacionesRevisadas;

                bool algunoRechazado = algunaObraRechazada || algunaInvestigacionRechazada ||
                                      algunCursoRechazado || algunaEvaluacionRechazada;

                cumpleRequisitos = todosRevisados && algunoRechazado;

                if (!cumpleRequisitos)
                {
                    if (!todosRevisados)
                    {
                        mensaje = "No se puede rechazar la postulación porque no todos los requisitos han sido revisados";
                    }
                    else if (!algunoRechazado)
                    {
                        mensaje = "No se puede rechazar la postulación porque no hay ningún requisito rechazado";
                    }
                }
            }

            // 5. Actualizar la postulación si cumple con los requisitos
            if (cumpleRequisitos)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Actualizar el estado de la postulación y guardar el revisor
                        ultimaPostulacion.EstPos = dto.Estado;
                        ultimaPostulacion.ObsPos = dto.Observacion;
                        ultimaPostulacion.Revisor = usuarioActual;

                        // Si la postulación es aprobada, actualizar el historial docente
                        if (dto.Estado == "APROBADO")
                        {
                            // Obtener información del docente para el PDF
                            var docente = await _context.Docentes
                                .FirstOrDefaultAsync(d => d.CedDoc == dto.CedulaDocente);

                            string nombreDocente = docente?.Nom1Doc ?? "Nombre no disponible";

                            // Obtener la información de la categoría actual
                            var categoriaActual = await _context.Categorias
                                .FirstOrDefaultAsync(c => c.IdCat == categoriaActualId);

                            if (categoriaActual == null)
                            {
                                return BadRequest("No se pudo encontrar la categoría actual del docente");
                            }

                            // Obtener la siguiente categoría
                            var siguienteCategoria = await _context.Categorias
                                .Where(c => c.NivCat > categoriaActual.NivCat)
                                .OrderBy(c => c.NivCat)
                                .FirstOrDefaultAsync();

                            if (siguienteCategoria == null)
                            {
                                return BadRequest("El docente ya está en la categoría más alta, no se puede promover");
                            }

                            // 1. Actualizar el registro actual en el historial estableciendo la fecha fin
                            ultimoHistorial.FecFin = fechaActual;

                            // 2. Generar PDF de certificado de promoción
                            byte[] pdfBytes = PdfHelper.GenerarPdfPostulacion(
                                dto.CedulaDocente,
                                nombreDocente,
                                categoriaActual.NomCat,
                                siguienteCategoria.NomCat,
                                fechaActual,
                                usuarioActual // Usar el usuario del JSON
                            );

                            // 3. Crear nuevo registro en el historial con la nueva categoría
                            var nuevoHistorial = new HistorialDocente
                            {
                                CedDoc = dto.CedulaDocente,
                                IdCat = siguienteCategoria.IdCat,
                                FecIni = fechaActual,
                                FecFin = null, // Fecha fin abierta
                                DocumentoPdf = pdfBytes
                            };

                            _context.HistorialDocentes.Add(nuevoHistorial);
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        if (dto.Estado == "APROBADO")
                        {
                            return Ok("La postulación ha sido aprobada y el docente ha sido promovido a la siguiente categoría");
                        }
                        else
                        {
                            return Ok($"La postulación ha sido {dto.Estado.ToLower()} correctamente");
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
                    }
                }
            }
            else
            {
                return BadRequest(mensaje);
            }
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardDataDto>> GetDashboardData()
        {
            try
            {
                var result = new DashboardDataDto();

                // 1. Obtener estadísticas - CORREGIDO para reflejar los valores correctos
                result.Estadisticas = new DashboardEstadisticasDto
                {
                    TotalDocentes = await _context.Docentes.CountAsync(),

                    EvaluacionesPendientes = await _context.Obras.CountAsync(o => o.Estado == "PENDIENTE") +
                                           await _context.CursosCapacitacions.CountAsync(c => c.Estado == "PENDIENTE") +
                                           await _context.Investigaciones.CountAsync(i => i.Estado == "PENDIENTE") +
                                           await _context.Evaluaciones.CountAsync(e => e.Estado == "PENDIENTE"),

                    // CAMBIO: Solo contar las APROBADAS como finalizadas
                    SolicitudesFinalizadas = await _context.Postulaciones.CountAsync(p => p.EstPos == "APROBADO"),

                    // CAMBIO: Contar las rechazadas
                    SolicitudesRechazadas = await _context.Postulaciones.CountAsync(p => p.EstPos == "RECHAZADO"),

                    // Mantener procesos activos
                    ProcesosActivos = await _context.Postulaciones.CountAsync(p => p.EstPos == "EN PROCESO")
                };

                // 2. Obtener postulaciones en REVISION
                var postulacionesRevision = await _context.Postulaciones
                    .Where(p => p.EstPos == "REVISION")
                    .Include(p => p.CedDocNavigation)
                    .ToListAsync();

                // 3. Obtener todas las categorías
                var categorias = await _context.Categorias.ToListAsync();

                // 4. Procesar cada postulación
                result.Solicitudes = new List<SolicitudDocenteDto>();

                foreach (var postulacion in postulacionesRevision)
                {
                    // Buscar categoría actual de la postulación
                    var categoriaActual = categorias.FirstOrDefault(c => c.IdCat == postulacion.IdCat);

                    if (categoriaActual != null)
                    {
                        // Buscar siguiente categoría basada en nivel
                        var siguienteCategoria = categorias
                            .Where(c => c.NivCat > categoriaActual.NivCat)
                            .OrderBy(c => c.NivCat)
                            .FirstOrDefault() ?? categoriaActual;

                        // Crear objeto de respuesta
                        result.Solicitudes.Add(new SolicitudDocenteDto
                        {
                            Id = postulacion.IdPos,
                            Codigo = postulacion.CedDoc, // Usar la cédula del docente
                            NombreDocente = $"{postulacion.CedDocNavigation.Nom1Doc} {postulacion.CedDocNavigation.Ape1Doc}",
                            Nivel = $"{categoriaActual.NomCat} a {siguienteCategoria.NomCat}",
                            Fecha = postulacion.FecPos,
                            TiempoEspera = CalcularTiempoEspera(postulacion.FecPos),
                            Estado = postulacion.EstPos
                        });
                    }
                }

                // 5. Ordenar por fecha descendente
                result.Solicitudes = result.Solicitudes.OrderByDescending(s => s.Fecha).ToList();

                Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Dashboard cargado por: materubag");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return StatusCode(500, $"Error al obtener datos del dashboard: {ex.Message}");
            }
        }

        // POST: api/Postulaciones/ValidarSolicitud/5
        [HttpPost("ValidarSolicitud/{id}")]
        public async Task<IActionResult> ValidarSolicitud(int id, [FromBody] ValidacionDto validacion)
        {
            try
            {
                var postulacion = await _context.Postulaciones.FindAsync(id);
                if (postulacion == null)
                    return NotFound($"No se encontró la postulación con ID {id}");

                // Actualizar la postulación
                postulacion.EstPos = "REVISION";
                postulacion.Revisor = validacion.Usuario;
                postulacion.FecPos = DateOnly.FromDateTime(DateTime.UtcNow);

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al validar solicitud: {ex.Message}");
            }
        }

        private static string CalcularTiempoEspera(DateOnly fecha)
        {
            DateTime fechaPostulacion = fecha.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            TimeSpan tiempoTranscurrido = DateTime.UtcNow - fechaPostulacion;

            if (tiempoTranscurrido.TotalDays < 1)
                return $"{(int)tiempoTranscurrido.TotalHours} horas";
            else if (tiempoTranscurrido.TotalDays < 30)
                return $"{(int)tiempoTranscurrido.TotalDays} días";
            else if (tiempoTranscurrido.TotalDays < 365)
                return $"{(int)(tiempoTranscurrido.TotalDays / 30)} meses";
            else
                return $"{(int)(tiempoTranscurrido.TotalDays / 365)} años";
        }
    }

}

