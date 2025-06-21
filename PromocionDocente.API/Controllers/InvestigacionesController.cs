using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Infrastructure.Utils;
using PromocionDocente.Models.PromocionDocenteModels;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigacionesController : ControllerBase
    {
        private readonly PromociondocenteContext _context;

        public InvestigacionesController(PromociondocenteContext context)
        {
            _context = context;
        }

        // GET: api/Investigaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Investigacione>>> GetInvestigaciones()
        {
            return await _context.Investigaciones.ToListAsync();
        }

        // GET: api/Investigaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Investigacione>> GetInvestigacione(int id)
        {
            var investigacione = await _context.Investigaciones.FindAsync(id);

            if (investigacione == null)
            {
                return NotFound();
            }

            return investigacione;
        }

        // GET: api/Investigaciones/cedula/{cedula}
        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<IEnumerable<object>>> GetInvestigacionesPorCedula(string cedula)
        {
            var investigaciones = await _context.Investigaciones
                                                .Where(i => i.CedDoc == cedula)
                                                .ToListAsync();

            if (investigaciones == null || !investigaciones.Any())
                return NotFound();

            var resultado = investigaciones.Select(i => new
            {
                i.IdInvestigacion,
                i.CedDoc,
                i.TituloInvestigacion,
                i.DuracionMeses,
                i.FechaInicio,
                i.FechaFin,
                i.TipoInvestigacion,
                i.CampoAplicacion,
                ArchivoPdf = i.ArchivoPdf != null
                    ? PdfHelper.GuardarBinarioComoPdf(i.ArchivoPdf, i.CedDoc, i.TituloInvestigacion, "Investigaciones")
                    : ""
            });

            return Ok(resultado);
        }

        // PUT: api/Investigaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvestigacione(int id, Investigacione investigacione)
        {
            if (id != investigacione.IdInvestigacion)
            {
                return BadRequest();
            }

            _context.Entry(investigacione).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvestigacioneExists(id))
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

        // POST: api/Investigaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Investigacione>> PostInvestigacione(Investigacione investigacione)
        {
            _context.Investigaciones.Add(investigacione);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvestigacione", new { id = investigacione.IdInvestigacion }, investigacione);
        }

        // DELETE: api/Investigaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvestigacione(int id)
        {
            var investigacione = await _context.Investigaciones.FindAsync(id);
            if (investigacione == null)
            {
                return NotFound();
            }

            _context.Investigaciones.Remove(investigacione);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvestigacioneExists(int id)
        {
            return _context.Investigaciones.Any(e => e.IdInvestigacion == id);
        }
    }
}
