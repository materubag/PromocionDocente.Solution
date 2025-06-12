using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.PromocionDocenteModels;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePostulacionsController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public DetallePostulacionsController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/DetallePostulacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePostulacion>>> GetDetallePostulacions()
        {
            return await _context.DetallePostulacions.ToListAsync();
        }

        // GET: api/DetallePostulacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePostulacion>> GetDetallePostulacion(int id)
        {
            var detallePostulacion = await _context.DetallePostulacions.FindAsync(id);

            if (detallePostulacion == null)
            {
                return NotFound();
            }

            return detallePostulacion;
        }

        // PUT: api/DetallePostulacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePostulacion(int id, DetallePostulacion detallePostulacion)
        {
            if (id != detallePostulacion.IdDet)
            {
                return BadRequest();
            }

            _context.Entry(detallePostulacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePostulacionExists(id))
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

        // POST: api/DetallePostulacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetallePostulacion>> PostDetallePostulacion(DetallePostulacion detallePostulacion)
        {
            _context.DetallePostulacions.Add(detallePostulacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetallePostulacion", new { id = detallePostulacion.IdDet }, detallePostulacion);
        }

        // DELETE: api/DetallePostulacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePostulacion(int id)
        {
            var detallePostulacion = await _context.DetallePostulacions.FindAsync(id);
            if (detallePostulacion == null)
            {
                return NotFound();
            }

            _context.DetallePostulacions.Remove(detallePostulacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallePostulacionExists(int id)
        {
            return _context.DetallePostulacions.Any(e => e.IdDet == id);
        }
    }
}
