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
            var fechaActual = DateTime.UtcNow; // 2025-06-24 04:31:49 basado en el timestamp proporcionado
            var usuarioActual = "materubag"; // Usuario actual del sistema

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

        [HttpPut("evaluar")]
        public async Task<IActionResult> AprobarRechazarPostulacionPorCedula(PostulacionAprobacionDto dto)
        {
            // Registra usuario y fecha actual
            var usuarioActual = "materubag";
            var fechaActual = DateOnly.FromDateTime(DateTime.UtcNow);

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

            // 3. Validar según el estado solicitado
            bool cumpleRequisitos = false;
            string mensaje = "";

            if (dto.Estado == "APROBADO")
            {
                // Verificar que todas las obras, investigaciones, cursos y evaluaciones estén aprobadas
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
                // Verificar que al menos un requisito esté rechazado y todos estén revisados

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

            // 4. Actualizar la postulación si cumple con los requisitos
            if (cumpleRequisitos)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Actualizar el estado de la postulación
                        ultimaPostulacion.EstPos = dto.Estado;
                        ultimaPostulacion.ObsPos = dto.Observacion;

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
                                usuarioActual
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
    }
}
