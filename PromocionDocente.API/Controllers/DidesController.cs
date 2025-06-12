using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.DIDE_Models;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DidesController : ControllerBase
    {
        private readonly DideContext _context;

        public DidesController(DideContext context)
        {
            _context = context;
        }

        // GET: api/Dides
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dide>>> GetDides()
        {
            return await _context.Dides.ToListAsync();
        }

        // GET: api/Dides/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dide>> GetDide(int id)
        {
            var dide = await _context.Dides.FindAsync(id);

            if (dide == null)
            {
                return NotFound();
            }

            return dide;
        }

        // PUT: api/Dides/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDide(int id, Dide dide)
        {
            if (id != dide.IdInvestigacion)
            {
                return BadRequest();
            }

            _context.Entry(dide).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DideExists(id))
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

        // POST: api/Dides
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dide>> PostDide(Dide dide)
        {
            _context.Dides.Add(dide);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDide", new { id = dide.IdInvestigacion }, dide);
        }

        // DELETE: api/Dides/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDide(int id)
        {
            var dide = await _context.Dides.FindAsync(id);
            if (dide == null)
            {
                return NotFound();
            }

            _context.Dides.Remove(dide);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DideExists(int id)
        {
            return _context.Dides.Any(e => e.IdInvestigacion == id);
        }
    }
}
