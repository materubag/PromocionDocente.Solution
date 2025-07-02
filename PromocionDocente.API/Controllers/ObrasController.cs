using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Infrastructure.Services;
using PromocionDocente.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObrasController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;
        private readonly ObraService _obraService;


        public ObrasController(PromocionDocenteContext context, ObraService obraService)
        {
            _context = context;
            _obraService = obraService;
        }


        // GET: api/Obras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obra>>> GetObras()
        {
            return await _context.Obras.ToListAsync();
        }

        // GET: api/Obras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Obra>> GetObra(int id)
        {
            var obra = await _context.Obras.FindAsync(id);

            if (obra == null)
            {
                return NotFound();
            }

            return obra;
        }

        // PUT: api/Obras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarObra(int id, [FromBody] ObraUpdateDto dto)
        {
            await _obraService.ActualizarObraAsync(id, dto);
            return NoContent();
        }




        // POST: api/Obras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostObra([FromBody] ObraDto dto)
        {
            var obra = new Obra
            {
                CedDoc = dto.CedDoc,
                TipoObra = dto.TipoObra,
                Titulo = dto.Titulo,
                FechaPublicacion = DateOnly.FromDateTime(dto.FechaPublicacion),
                DoiUrl = dto.DoiUrl,
                AreaConocimiento = dto.AreaConocimiento,
                Observaciones = dto.Observaciones,
                PdfProduccion = dto.PdfProduccion,
                Estado = "PENDIENTE",
            };

            _context.Obras.Add(obra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetObra), new { id = obra.IdObra }, obra);
        }



        // DELETE: api/Obras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObra(int id)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null)
            {
                return NotFound();
            }

            _context.Obras.Remove(obra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObraExists(int id)
        {
            return _context.Obras.Any(e => e.IdObra == id);
        }

        [HttpPut("estado/{id}")]
        public async Task<IActionResult> UpdateEstadoObra(int id, EstadoUpdateDto obraDto)
        {
            // 1. Verificar que la obra existe
            var obra = await _context.Obras
                .Where(o => o.IdObra == id)
                .Select(o => new { o.CedDoc, o.Estado })
                .FirstOrDefaultAsync();

            if (obra == null)
            {
                return NotFound($"No se encontró la obra con ID {id}");
            }

            // 2. Verificar si se está intentando actualizar al mismo estado
            if (obra.Estado == obraDto.Estado)
            {
                return BadRequest($"La obra ya tiene el estado '{obraDto.Estado}'. No se realizaron cambios.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 3. Actualizar estado en OBRAS
                    string sqlUpdate = @"
                UPDATE OBRAS 
                SET ESTADO = @estado, 
                    OBSERVACION = @observacion 
                WHERE ID_OBRA = @id";

                    var parametrosUpdate = new[]
                    {
                new SqlParameter("@estado", obraDto.Estado),
                new SqlParameter("@observacion", (object)obraDto.Observacion ?? DBNull.Value),
                new SqlParameter("@id", id)
            };

                    await _context.Database.ExecuteSqlRawAsync(sqlUpdate, parametrosUpdate);

                    // 4. Registrar en DETALLE_POSTULACION si el estado es APROBADO o RECHAZADO
                    if (obraDto.Estado == "APROBADO" || obraDto.Estado == "RECHAZADO")
                    {
                        // Obtener la última postulación del docente
                        var ultimaPostulacion = await _context.Postulaciones
                            .Where(p => p.CedDoc == obra.CedDoc)
                            .OrderByDescending(p => p.FecPos)
                            .Select(p => p.IdPos)
                            .FirstOrDefaultAsync();

                        if (ultimaPostulacion == 0)
                        {
                            await transaction.RollbackAsync();
                            return BadRequest($"No se encontró una postulación para el docente con cédula {obra.CedDoc}");
                        }

                        // Insertar en DETALLE_POSTULACION
                        string sqlInsert = @"
                    INSERT INTO DETALLE_POSTULACION 
                    (ID_POS, OBSERVACION, ESTADO, TABLA_ORIGEN, ID_ORIGEN)
                    VALUES (@idPos, @observacion, @estado, @tablaOrigen, @idOrigen)";

                        var parametrosInsert = new[]
                        {
                    new SqlParameter("@idPos", ultimaPostulacion),
                    new SqlParameter("@observacion", string.IsNullOrEmpty(obraDto.Observacion) ? "Sin observaciones" : obraDto.Observacion),
                    new SqlParameter("@estado", obraDto.Estado),
                    new SqlParameter("@tablaOrigen", "OBRAS"),
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
