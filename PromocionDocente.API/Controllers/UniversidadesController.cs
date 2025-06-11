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
    public class UniversidadesController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public UniversidadesController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/Universidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Universidade>>> GetUniversidades()
        {
            return await _context.Universidades.ToListAsync();
        }

        // GET: api/Universidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Universidade>> GetUniversidade(string id)
        {
            var universidade = await _context.Universidades.FindAsync(id);

            if (universidade == null)
            {
                return NotFound();
            }

            return universidade;
        }

        // PUT: api/Universidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUniversidade(string id, Universidade universidade)
        {
            if (id != universidade.IdUni)
            {
                return BadRequest();
            }

            _context.Entry(universidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UniversidadeExists(id))
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

        // POST: api/Universidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Universidade>> PostUniversidade(Universidade universidade)
        {
            _context.Universidades.Add(universidade);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UniversidadeExists(universidade.IdUni))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUniversidade", new { id = universidade.IdUni }, universidade);
        }

        // DELETE: api/Universidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUniversidade(string id)
        {
            var universidade = await _context.Universidades.FindAsync(id);
            if (universidade == null)
            {
                return NotFound();
            }

            _context.Universidades.Remove(universidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UniversidadeExists(string id)
        {
            return _context.Universidades.Any(e => e.IdUni == id);
        }
    }
}
