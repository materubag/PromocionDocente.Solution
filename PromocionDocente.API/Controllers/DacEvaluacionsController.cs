using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.DAC_Models;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DacEvaluacionsController : ControllerBase
    {
        private readonly DacContext _context;

        public DacEvaluacionsController(DacContext context)
        {
            _context = context;
        }

        // GET: api/DacEvaluacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DacEvaluacion>>> GetDacEvaluacions()
        {
            return await _context.DacEvaluacions.ToListAsync();
        }

        // GET: api/DacEvaluacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DacEvaluacion>> GetDacEvaluacion(int id)
        {
            var dacEvaluacion = await _context.DacEvaluacions.FindAsync(id);

            if (dacEvaluacion == null)
            {
                return NotFound();
            }

            return dacEvaluacion;
        }

        // PUT: api/DacEvaluacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDacEvaluacion(int id, DacEvaluacion dacEvaluacion)
        {
            if (id != dacEvaluacion.IdEvaluacion)
            {
                return BadRequest();
            }

            _context.Entry(dacEvaluacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DacEvaluacionExists(id))
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

        // POST: api/DacEvaluacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DacEvaluacion>> PostDacEvaluacion(DacEvaluacion dacEvaluacion)
        {
            _context.DacEvaluacions.Add(dacEvaluacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDacEvaluacion", new { id = dacEvaluacion.IdEvaluacion }, dacEvaluacion);
        }

        // DELETE: api/DacEvaluacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDacEvaluacion(int id)
        {
            var dacEvaluacion = await _context.DacEvaluacions.FindAsync(id);
            if (dacEvaluacion == null)
            {
                return NotFound();
            }

            _context.DacEvaluacions.Remove(dacEvaluacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DacEvaluacionExists(int id)
        {
            return _context.DacEvaluacions.Any(e => e.IdEvaluacion == id);
        }
    }
}
