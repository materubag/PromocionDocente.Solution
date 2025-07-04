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
    public class InvestigacionesController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public InvestigacionesController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/Investigaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Investigacione>>> GetInvestigaciones()
        {
            return await _context.Investigaciones.ToListAsync();
        }

        // GET: api/Investigaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Investigacione>> GetInvestigacione(int id)
        {
            var investigacione = await _context.Investigaciones.FindAsync(id);

            if (investigacione == null)
            {
                return NotFound();
            }

            return investigacione;
        }

        // GET: api/Investigaciones/cedula/{cedula}
        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<IEnumerable<object>>> GetInvestigacionesPorCedula(string cedula)
        {
            var investigaciones = await _context.Investigaciones
                                                .Where(i => i.CedDoc == cedula)
                                                .ToListAsync();

            if (investigaciones == null || !investigaciones.Any())
                return NotFound();

            var resultado = investigaciones.Select(i => new
            {
                i.IdInvestigacion,
                i.CedDoc,
                i.TituloInvestigacion,
                i.DuracionMeses,
                i.FechaInicio,
                i.FechaFin,
                i.TipoInvestigacion,
                i.CampoAplicacion,
                i.Estado,
                ArchivoPdf = i.ArchivoPdf != null
                    ? PdfHelper.GuardarBinarioComoPdf(i.ArchivoPdf, i.CedDoc, i.TituloInvestigacion, "Investigaciones")
                    : ""
            });

            return Ok(resultado);
        }

        // PUT: api/Investigaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvestigacione(int id, Investigacione investigacione)
        {
            if (id != investigacione.IdInvestigacion)
            {
                return BadRequest();
            }

            _context.Entry(investigacione).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvestigacioneExists(id))
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

        // POST: api/Investigaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Investigacione>> PostInvestigacione(Investigacione investigacione)
        {
            _context.Investigaciones.Add(investigacione);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvestigacione", new { id = investigacione.IdInvestigacion }, investigacione);
        }

        // DELETE: api/Investigaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvestigacione(int id)
        {
            var investigacione = await _context.Investigaciones.FindAsync(id);
            if (investigacione == null)
            {
                return NotFound();
            }

            _context.Investigaciones.Remove(investigacione);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvestigacioneExists(int id)
        {
            return _context.Investigaciones.Any(e => e.IdInvestigacion == id);
        }

        [HttpPut("estado/{id}")]
        public async Task<IActionResult> UpdateEstadoInvestigacion(int id, EstadoUpdateDto investigacionDto)
        {
            // 1. Verificar que la investigación existe
            var investigacion = await _context.Investigaciones
                .Where(i => i.IdInvestigacion == id)
                .Select(i => new { i.CedDoc, i.Estado })
                .FirstOrDefaultAsync();

            if (investigacion == null)
            {
                return NotFound($"No se encontró la investigación con ID {id}");
            }

            // 2. Verificar si se está intentando actualizar al mismo estado
            if (investigacion.Estado == investigacionDto.Estado)
            {
                return BadRequest($"La investigación ya tiene el estado '{investigacionDto.Estado}'. No se realizaron cambios.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 3. Actualizar estado en INVESTIGACIONES
                    string sqlUpdate = @"
                UPDATE INVESTIGACIONES 
                SET ESTADO = @estado, 
                    OBSERVACION = @observacion 
                WHERE ID_INVESTIGACION = @id";

                    var parametrosUpdate = new[]
                    {
                new SqlParameter("@estado", investigacionDto.Estado),
                new SqlParameter("@observacion", (object)investigacionDto.Observacion ?? DBNull.Value),
                new SqlParameter("@id", id)
            };

                    await _context.Database.ExecuteSqlRawAsync(sqlUpdate, parametrosUpdate);

                    // 4. Registrar en DETALLE_POSTULACION si el estado es APROBADO o RECHAZADO
                    if (investigacionDto.Estado == "APROBADO" || investigacionDto.Estado == "RECHAZADO")
                    {
                        // Obtener la última postulación del docente
                        var ultimaPostulacion = await _context.Postulaciones
                            .Where(p => p.CedDoc == investigacion.CedDoc)
                            .OrderByDescending(p => p.FecPos)
                            .Select(p => p.IdPos)
                            .FirstOrDefaultAsync();

                        if (ultimaPostulacion == 0)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest($"No se encontró una postulación para el docente con cédula {investigacion.CedDoc}");
                        }

                        // Insertar en DETALLE_POSTULACION
                        string sqlInsert = @"
                    INSERT INTO DETALLE_POSTULACION 
                    (ID_POS, OBSERVACION, ESTADO, TABLA_ORIGEN, ID_ORIGEN)
                    VALUES (@idPos, @observacion, @estado, @tablaOrigen, @idOrigen)";

                        var parametrosInsert = new[]
                        {
                    new SqlParameter("@idPos", ultimaPostulacion),
                    new SqlParameter("@observacion", string.IsNullOrEmpty(investigacionDto.Observacion) ? "Sin observaciones" : investigacionDto.Observacion),
                    new SqlParameter("@estado", investigacionDto.Estado),
                    new SqlParameter("@tablaOrigen", "INVESTIGACIONES"),
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
    }
}
