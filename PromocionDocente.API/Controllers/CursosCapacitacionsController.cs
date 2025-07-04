using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Application.DTOs.Update;
using PromocionDocente.Application.Services;
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
    public class CursosCapacitacionsController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;
        private readonly CursoCapacitacionService _cursoService;

        public CursosCapacitacionsController(PromocionDocenteContext context,CursoCapacitacionService cursoCapacitacionService)
        {
            _context = context;
            _cursoService = cursoCapacitacionService;
        }

        // GET: api/CursosCapacitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursosCapacitacion>>> GetCursosCapacitacions()
        {
            return await _context.CursosCapacitacions.ToListAsync();
        }

        // GET: api/CursosCapacitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursosCapacitacion>> GetCursosCapacitacion(int id)
        {
            var cursosCapacitacion = await _context.CursosCapacitacions.FindAsync(id);

            if (cursosCapacitacion == null)
            {
                return NotFound();
            }

            return cursosCapacitacion;
        }

        [HttpGet("cedula/{cedDoc}")]
        public async Task<ActionResult<List<object>>> GetCursosPorCedula(string cedDoc)
        {
            var cursos = await _context.CursosCapacitacions
                                       .Where(c => c.CedDoc == cedDoc)
                                       .ToListAsync();

            if (cursos == null || cursos.Count == 0)
                return NotFound();

            var resultado = cursos.Select(c => new
            {
                c.IdCurso,
                c.CedDoc,
                c.NombreCurso,
                c.FechaCurso,
                c.Horas,
                c.Estado,
                PdfCurso = c.PdfCurso != null
                    ? PdfHelper.GuardarBinarioComoPdf(c.PdfCurso, c.CedDoc, c.NombreCurso, "Cursos")
                    : ""
            }).ToList();

            return Ok(resultado);
        }


        // PUT: api/CursosCapacitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, [FromBody] CursoCapacitacionUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos.");

            var curso = await _context.CursosCapacitacions.FindAsync(id);
            if (curso == null)
                return NotFound($"No se encontró el curso con ID {id}.");

            curso.NombreCurso = dto.NombreCurso;
            curso.Horas = dto.Horas;
            curso.FechaCurso = DateOnly.FromDateTime(dto.FechaCurso);
            curso.PdfCurso = dto.PdfCurso;
            curso.Observacion = dto.Observacion;

            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpPut("estado/{id}")]
        public async Task<IActionResult> UpdateEstadoCurso(int id, EstadoUpdateDto cursoDto)
        {
            // 1. Verificar que el curso existe
            var curso = await _context.CursosCapacitacions
                .Where(c => c.IdCurso == id)
                .Select(c => new { c.CedDoc, c.Estado })
                .FirstOrDefaultAsync();

            if (curso == null)
            {
                return NotFound($"No se encontró el curso con ID {id}");
            }

            // 2. Verificar si se está intentando actualizar al mismo estado
            if (curso.Estado == cursoDto.Estado)
            {
                return BadRequest($"El curso ya tiene el estado '{cursoDto.Estado}'. No se realizaron cambios.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 3. Actualizar estado en CURSOS_CAPACITACION
                    string sqlUpdate = @"
                UPDATE CURSOS_CAPACITACION 
                SET ESTADO = @estado, 
                    OBSERVACION = @observacion 
                WHERE ID_CURSO = @id";

                    var parametrosUpdate = new[]
                    {
                new SqlParameter("@estado", cursoDto.Estado),
                new SqlParameter("@observacion", (object)cursoDto.Observacion ?? DBNull.Value),
                new SqlParameter("@id", id)
            };

                    await _context.Database.ExecuteSqlRawAsync(sqlUpdate, parametrosUpdate);

                    // 4. Registrar en DETALLE_POSTULACION si el estado es APROBADO o RECHAZADO
                    if (cursoDto.Estado == "APROBADO" || cursoDto.Estado == "RECHAZADO")
                    {
                        // Obtener la última postulación del docente
                        var ultimaPostulacion = await _context.Postulaciones
                            .Where(p => p.CedDoc == curso.CedDoc)
                            .OrderByDescending(p => p.FecPos)
                            .Select(p => p.IdPos)
                            .FirstOrDefaultAsync();

                        if (ultimaPostulacion == 0)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest($"No se encontró una postulación para el docente con cédula {curso.CedDoc}");
                        }

                        // Insertar en DETALLE_POSTULACION
                        string sqlInsert = @"
                    INSERT INTO DETALLE_POSTULACION 
                    (ID_POS, OBSERVACION, ESTADO, TABLA_ORIGEN, ID_ORIGEN)
                    VALUES (@idPos, @observacion, @estado, @tablaOrigen, @idOrigen)";

                        var parametrosInsert = new[]
                        {
                    new SqlParameter("@idPos", ultimaPostulacion),
                    new SqlParameter("@observacion", string.IsNullOrEmpty(cursoDto.Observacion) ? "Sin observaciones" : cursoDto.Observacion),
                    new SqlParameter("@estado", cursoDto.Estado),
                    new SqlParameter("@tablaOrigen", "CURSOS_CAPACITACION"),
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



        // POST: api/CursosCapacitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostCursosCapacitacion([FromBody] CursoCapacitacionDto dto)
        {
            // Mapear DTO -> Entidad
            var curso = new CursosCapacitacion
            {
                CedDoc = dto.CedulaDocente,
                NombreCurso = dto.NombreCurso,
                FechaCurso = DateOnly.FromDateTime(dto.FechaCurso),
                Horas = dto.Horas,
                PdfCurso = dto.PdfCurso,
                Estado = "PENDIENTE",
                Observacion = dto.Observacion
            };

            _context.CursosCapacitacions.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCursosCapacitacion), new { id = curso.IdCurso }, curso);
        }


        // DELETE: api/CursosCapacitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCursosCapacitacion(int id)
        {
            var cursosCapacitacion = await _context.CursosCapacitacions.FindAsync(id);
            if (cursosCapacitacion == null)
            {
                return NotFound();
            }

            _context.CursosCapacitacions.Remove(cursosCapacitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursosCapacitacionExists(int id)
        {
            return _context.CursosCapacitacions.Any(e => e.IdCurso == id);
        }
    }
}
