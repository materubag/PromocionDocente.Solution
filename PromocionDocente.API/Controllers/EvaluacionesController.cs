using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Application.DTOs.Update;
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
    public class EvaluacionesController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public EvaluacionesController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/Evaluaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evaluacione>>> GetEvaluaciones()
        {
            return await _context.Evaluaciones.ToListAsync();
        }

        // GET: api/Evaluaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evaluacione>> GetEvaluacione(int id)
        {
            var evaluacione = await _context.Evaluaciones.FindAsync(id);

            if (evaluacione == null)
            {
                return NotFound();
            }

            return evaluacione;
        }

       


        // PUT: api/Evaluaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacione(int id, [FromBody] EvaluacionUpdateDto dto)
        {
            var evaluacion = await _context.Evaluaciones.FindAsync(id);
            if (evaluacion == null)
                return NotFound();

            evaluacion.PeriodoEvaluado = dto.PeriodoEvaluado;
            evaluacion.TipoEvaluacion = dto.TipoEvaluacion;
            evaluacion.FechaEvaluacion = DateOnly.FromDateTime(dto.FechaEvaluacion); // aquí conviertes
            evaluacion.Resultado = dto.Resultado;
            evaluacion.Observacion = dto.Observaciones;
            evaluacion.PdfEvaluacion = dto.PdfEvaluacion; 


            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Evaluaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostEvaluacion([FromBody] EvaluacionDto dto)
        {
            var evaluacion = new Evaluacione
            {
                CedDoc = dto.CedDoc,
                PeriodoEvaluado = dto.PeriodoEvaluado,
                TipoEvaluacion = dto.TipoEvaluacion,
                FechaEvaluacion = DateOnly.FromDateTime(dto.FechaEvaluacion),
                Resultado = dto.Resultado,
                Observacion = dto.Observacion,
                PdfEvaluacion = dto.PdfEvaluacion,
                Estado = "PENDIENTE"
            };

            _context.Evaluaciones.Add(evaluacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvaluacione), new { id = evaluacion.IdEvaluacion }, evaluacion);
        }

        // DELETE: api/Evaluaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacione(int id)
        {
            var evaluacione = await _context.Evaluaciones.FindAsync(id);
            if (evaluacione == null)
            {
                return NotFound();
            }

            _context.Evaluaciones.Remove(evaluacione);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacioneExists(int id)
        {
            return _context.Evaluaciones.Any(e => e.IdEvaluacion == id);
        }


        [HttpPut("estado/{id}")]
        public async Task<IActionResult> UpdateEstadoEvaluacion(int id, EstadoUpdateDto evaluacionDto)
        {
            // 1. Verificar que la evaluación existe
            var evaluacion = await _context.Evaluaciones
                .Where(e => e.IdEvaluacion == id)
                .Select(e => new { e.CedDoc, e.Estado })
                .FirstOrDefaultAsync();

            if (evaluacion == null)
            {
                return NotFound($"No se encontró la evaluación con ID {id}");
            }

            // 2. Verificar si se está intentando actualizar al mismo estado
            if (evaluacion.Estado == evaluacionDto.Estado)
            {
                return BadRequest($"La evaluación ya tiene el estado '{evaluacionDto.Estado}'. No se realizaron cambios.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 3. Actualizar estado en EVALUACIONES
                    string sqlUpdate = @"
                UPDATE EVALUACIONES 
                SET ESTADO = @estado, 
                    OBSERVACION = @observacion 
                WHERE ID_EVALUACION = @id";

                    var parametrosUpdate = new[]
                    {
                new SqlParameter("@estado", evaluacionDto.Estado),
                new SqlParameter("@observacion", (object)evaluacionDto.Observacion ?? DBNull.Value),
                new SqlParameter("@id", id)
            };

                    await _context.Database.ExecuteSqlRawAsync(sqlUpdate, parametrosUpdate);

                    // 4. Registrar en DETALLE_POSTULACION si el estado es APROBADO o RECHAZADO
                    if (evaluacionDto.Estado == "APROBADO" || evaluacionDto.Estado == "RECHAZADO")
                    {
                        // Obtener la última postulación del docente
                        var ultimaPostulacion = await _context.Postulaciones
                            .Where(p => p.CedDoc == evaluacion.CedDoc)
                            .OrderByDescending(p => p.FecPos)
                            .Select(p => p.IdPos)
                            .FirstOrDefaultAsync();

                        if (ultimaPostulacion == 0)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest($"No se encontró una postulación para el docente con cédula {evaluacion.CedDoc}");
                        }

                        // Insertar en DETALLE_POSTULACION
                        string sqlInsert = @"
                    INSERT INTO DETALLE_POSTULACION 
                    (ID_POS, OBSERVACION, ESTADO, TABLA_ORIGEN, ID_ORIGEN)
                    VALUES (@idPos, @observacion, @estado, @tablaOrigen, @idOrigen)";

                        var parametrosInsert = new[]
                        {
                    new SqlParameter("@idPos", ultimaPostulacion),
                    new SqlParameter("@observacion", string.IsNullOrEmpty(evaluacionDto.Observacion) ? "Sin observaciones" : evaluacionDto.Observacion),
                    new SqlParameter("@estado", evaluacionDto.Estado),
                    new SqlParameter("@tablaOrigen", "EVALUACIONES"),
                    new SqlParameter("@idOrigen", id.ToString())
                };

                        await _context.Database.ExecuteSqlRawAsync(sqlInsert, parametrosInsert);
                    }

                    await transaction.CommitAsync();
                    return NoContent();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Error al actualizar el estado: {ex.Message}");
                }
            }
        }

        [HttpGet("cedula/{cedula}")]
        public async Task<IActionResult> GetEvaluacionesPorCedula(string cedula)
        {
            // 1. Buscar la fecha más reciente de evaluación para la cédula
            var ultimaFecha = await _context.HistorialDocentes
                                            .Where(e => e.CedDoc == cedula)
                                            .MaxAsync(e => (DateOnly?)e.FecIni);

            if (ultimaFecha == null)
                return NotFound("No se encontraron evaluaciones para la cédula especificada.");

            // 2. Traer todas las evaluaciones desde esa fecha (inclusive)
            var evaluaciones = await _context.Evaluaciones
                                             .Where(e => e.CedDoc == cedula && e.FechaEvaluacion >= ultimaFecha)
                                             .ToListAsync();

            if (evaluaciones.Count == 0)
                return NotFound("No hay evaluaciones a partir de la última fecha registrada.");

            // 3. Proyección anónima con la generación del PDF
            var resultado = evaluaciones.Select(e => new
            {
                e.IdEvaluacion,
                e.FechaEvaluacion,
                e.Resultado,
                e.CedDoc,
                e.TipoEvaluacion,
                e.PeriodoEvaluado,
                e.Estado,
                PdfEvaluacion = e.PdfEvaluacion != null
                    ? PdfHelper.GuardarBinarioComoPdf(
                          e.PdfEvaluacion,
                          e.CedDoc,
                          $"{e.TipoEvaluacion}_{e.PeriodoEvaluado}",
                          "Evaluaciones")
                    : string.Empty
            }).ToList();

            return Ok(resultado);
        }
    }
}
