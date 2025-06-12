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
    public class EvaluacionesController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public EvaluacionesController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/Evaluaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evaluacione>>> GetEvaluaciones()
        {
            return await _context.Evaluaciones.ToListAsync();
        }

        // GET: api/Evaluaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evaluacione>> GetEvaluacione(int id)
        {
            var evaluacione = await _context.Evaluaciones.FindAsync(id);

            if (evaluacione == null)
            {
                return NotFound();
            }

            return evaluacione;
        }

        // PUT: api/Evaluaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacione(int id, Evaluacione evaluacione)
        {
            if (id != evaluacione.IdEvaluacion)
            {
                return BadRequest();
            }

            _context.Entry(evaluacione).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacioneExists(id))
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

        // POST: api/Evaluaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evaluacione>> PostEvaluacione(Evaluacione evaluacione)
        {
            _context.Evaluaciones.Add(evaluacione);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacione", new { id = evaluacione.IdEvaluacion }, evaluacione);
        }

        // DELETE: api/Evaluaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacione(int id)
        {
            var evaluacione = await _context.Evaluaciones.FindAsync(id);
            if (evaluacione == null)
            {
                return NotFound();
            }

            _context.Evaluaciones.Remove(evaluacione);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacioneExists(int id)
        {
            return _context.Evaluaciones.Any(e => e.IdEvaluacion == id);
        }
    }
}
