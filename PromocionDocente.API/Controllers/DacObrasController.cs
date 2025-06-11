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
    public class DacObrasController : ControllerBase
    {
        private readonly DacContext _context;

        public DacObrasController(DacContext context)
        {
            _context = context;
        }

        // GET: api/DacObras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DacObra>>> GetDacObras()
        {
            return await _context.DacObras.ToListAsync();
        }

        // GET: api/DacObras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DacObra>> GetDacObra(int id)
        {
            var dacObra = await _context.DacObras.FindAsync(id);

            if (dacObra == null)
            {
                return NotFound();
            }

            return dacObra;
        }

        // PUT: api/DacObras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDacObra(int id, DacObra dacObra)
        {
            if (id != dacObra.IdObra)
            {
                return BadRequest();
            }

            _context.Entry(dacObra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DacObraExists(id))
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

        // POST: api/DacObras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DacObra>> PostDacObra(DacObra dacObra)
        {
            _context.DacObras.Add(dacObra);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDacObra", new { id = dacObra.IdObra }, dacObra);
        }

        // DELETE: api/DacObras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDacObra(int id)
        {
            var dacObra = await _context.DacObras.FindAsync(id);
            if (dacObra == null)
            {
                return NotFound();
            }

            _context.DacObras.Remove(dacObra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DacObraExists(int id)
        {
            return _context.DacObras.Any(e => e.IdObra == id);
        }
    }
}
