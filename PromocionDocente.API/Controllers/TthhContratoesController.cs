using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.TTHH_Models;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TthhContratoesController : ControllerBase
    {
        private readonly TthhContext _context;

        public TthhContratoesController(TthhContext context)
        {
            _context = context;
        }

        // GET: api/TthhContratoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TthhContrato>>> GetTthhContratos()
        {
            return await _context.TthhContratos.ToListAsync();
        }

        // GET: api/TthhContratoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TthhContrato>> GetTthhContrato(int id)
        {
            var tthhContrato = await _context.TthhContratos.FindAsync(id);

            if (tthhContrato == null)
            {
                return NotFound();
            }

            return tthhContrato;
        }

        // PUT: api/TthhContratoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTthhContrato(int id, TthhContrato tthhContrato)
        {
            if (id != tthhContrato.IdContrato)
            {
                return BadRequest();
            }

            _context.Entry(tthhContrato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TthhContratoExists(id))
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

        // POST: api/TthhContratoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TthhContrato>> PostTthhContrato(TthhContrato tthhContrato)
        {
            _context.TthhContratos.Add(tthhContrato);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTthhContrato", new { id = tthhContrato.IdContrato }, tthhContrato);
        }

        // DELETE: api/TthhContratoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTthhContrato(int id)
        {
            var tthhContrato = await _context.TthhContratos.FindAsync(id);
            if (tthhContrato == null)
            {
                return NotFound();
            }

            _context.TthhContratos.Remove(tthhContrato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TthhContratoExists(int id)
        {
            return _context.TthhContratos.Any(e => e.IdContrato == id);
        }
    }
}
