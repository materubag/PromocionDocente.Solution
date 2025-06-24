using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.Models;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulacionesController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public PostulacionesController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/Postulaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postulacione>>> GetPostulaciones()
        {
            return await _context.Postulaciones.ToListAsync();
        }

        // GET: api/Postulaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postulacione>> GetPostulacione(int id)
        {
            var postulacione = await _context.Postulaciones.FindAsync(id);

            if (postulacione == null)
            {
                return NotFound();
            }

            return postulacione;
        }

        // PUT: api/Postulaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostulacione(int id, Postulacione postulacione)
        {
            if (id != postulacione.IdPos)
            {
                return BadRequest();
            }

            _context.Entry(postulacione).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostulacioneExists(id))
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

        // POST: api/Postulaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Postulacione>> PostPostulacione(Postulacione postulacione)
        {
            _context.Postulaciones.Add(postulacione);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPostulacione", new { id = postulacione.IdPos }, postulacione);
        }

        // DELETE: api/Postulaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostulacione(int id)
        {
            var postulacione = await _context.Postulaciones.FindAsync(id);
            if (postulacione == null)
            {
                return NotFound();
            }

            _context.Postulaciones.Remove(postulacione);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostulacioneExists(int id)
        {
            return _context.Postulaciones.Any(e => e.IdPos == id);
        }
    }
}
