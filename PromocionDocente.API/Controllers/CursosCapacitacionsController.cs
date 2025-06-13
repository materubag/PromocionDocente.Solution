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
    public class CursosCapacitacionsController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public CursosCapacitacionsController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/CursosCapacitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursosCapacitacion>>> GetCursosCapacitacions()
        {
            return await _context.CursosCapacitacions.ToListAsync();
        }

        // GET: api/CursosCapacitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursosCapacitacion>> GetCursosCapacitacion(int id)
        {
            var cursosCapacitacion = await _context.CursosCapacitacions.FindAsync(id);

            if (cursosCapacitacion == null)
            {
                return NotFound();
            }

            return cursosCapacitacion;
        }

        [HttpGet("cedula/{cedDoc}")]
        public async Task<ActionResult<List<CursosCapacitacion>>> GetCursosPorCedula(string cedDoc)
        {
            // Buscar cursos donde la cédula coincida con cedDoc
            var cursos = await _context.CursosCapacitacions
                                       .Where(c => c.CedDoc == cedDoc)
                                       .ToListAsync();

            if (cursos == null || cursos.Count == 0)
            {
                return NotFound();
            }

            return cursos;
        }

        // PUT: api/CursosCapacitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCursosCapacitacion(int id, CursosCapacitacion cursosCapacitacion)
        {
            if (id != cursosCapacitacion.IdCurso)
            {
                return BadRequest();
            }

            _context.Entry(cursosCapacitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursosCapacitacionExists(id))
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

        // POST: api/CursosCapacitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CursosCapacitacion>> PostCursosCapacitacion(CursosCapacitacion cursosCapacitacion)
        {
            _context.CursosCapacitacions.Add(cursosCapacitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursosCapacitacion", new { id = cursosCapacitacion.IdCurso }, cursosCapacitacion);
        }

        // DELETE: api/CursosCapacitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCursosCapacitacion(int id)
        {
            var cursosCapacitacion = await _context.CursosCapacitacions.FindAsync(id);
            if (cursosCapacitacion == null)
            {
                return NotFound();
            }

            _context.CursosCapacitacions.Remove(cursosCapacitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursosCapacitacionExists(int id)
        {
            return _context.CursosCapacitacions.Any(e => e.IdCurso == id);
        }
    }
}
