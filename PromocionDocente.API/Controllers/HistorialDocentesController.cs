using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Infrastructure.Utils;
using PromocionDocente.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialDocentesController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public HistorialDocentesController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/HistorialDocentes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialDocente>>> GetHistorialDocentes()
        {
            return await _context.HistorialDocentes.ToListAsync();
        }

        // GET: api/HistorialDocentes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialDocente>> GetHistorialDocente(int id)
        {
            var historialDocente = await _context.HistorialDocentes.FindAsync(id);

            if (historialDocente == null)
            {
                return NotFound();
            }

            return historialDocente;
        }

        // GET: api/HistorialDocentes/cedula/{cedDoc}
        [HttpGet("cedula/{cedDoc}")]
        public async Task<ActionResult<List<object>>> GetHistorialDocentePorCedula(string cedDoc)
        {
            // Obtener todos los registros de historial para la cédula especificada
            // incluyendo datos de categoría y docente mediante joins
            var historiales = await (from h in _context.HistorialDocentes
                                     join c in _context.Categorias on h.IdCat equals c.IdCat
                                     join d in _context.Docentes on h.CedDoc equals d.CedDoc
                                     where h.CedDoc == cedDoc
                                     select new
                                     {
                                         h.IdHis,
                                         h.CedDoc,
                                         NombreDocente = $"{d.Nom1Doc} {d.Ape1Doc}",
                                         h.IdCat,
                                         NombreCategoria = c.NomCat, // Asumiendo que el campo de nombre en la tabla Categorias es "Nombre"
                                         h.FecIni,
                                         h.FecFin,
                                         DocumentoPdf = h.DocumentoPdf != null
                                             ? PdfHelper.GuardarBinarioComoPdf(h.DocumentoPdf, h.CedDoc, $"Historial_{h.IdHis}", "Historiales")
                                             : ""
                                     }).ToListAsync();

            if (historiales == null || historiales.Count == 0)
                return NotFound("No se encontró historial para la cédula especificada.");

            return Ok(historiales);
        }

        // PUT: api/HistorialDocentes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorialDocente(int id, HistorialDocente historialDocente)
        {
            if (id != historialDocente.IdHis)
            {
                return BadRequest();
            }

            _context.Entry(historialDocente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorialDocenteExists(id))
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

        // POST: api/HistorialDocentes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HistorialDocente>> PostHistorialDocente(HistorialDocente historialDocente)
        {
            _context.HistorialDocentes.Add(historialDocente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistorialDocente", new { id = historialDocente.IdHis }, historialDocente);
        }

        // DELETE: api/HistorialDocentes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorialDocente(int id)
        {
            var historialDocente = await _context.HistorialDocentes.FindAsync(id);
            if (historialDocente == null)
            {
                return NotFound();
            }

            _context.HistorialDocentes.Remove(historialDocente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistorialDocenteExists(int id)
        {
            return _context.HistorialDocentes.Any(e => e.IdHis == id);
        }


        [HttpGet("estadisticas/{cedula}")]
        public async Task<ActionResult<object>> GetEstadisticasDocente(string cedula)
        {
            var historial = await _context.HistorialDocentes
                .Where(h => h.CedDoc == cedula)
                .OrderByDescending(h => h.FecIni)
                .FirstOrDefaultAsync();

            if (historial == null)
            {
                return NotFound("No hay historial para esa cédula.");
            }

            var fechaInicio = historial.FecIni;
            var idNivelActual = historial.IdCat;

            var categoriaActual = await _context.Categorias
            .Where(c => c.IdCat == idNivelActual)
            .Select(c => new { c.NomCat, c.NivCat })
            .FirstOrDefaultAsync();

            string nombreNivelActual = categoriaActual.NomCat;

            var categoriaSiguiente = await _context.Categorias
                .Where(c => c.NivCat == categoriaActual.NivCat + 1)
                .Select(c => new { c.IdCat, c.NomCat })
                .FirstOrDefaultAsync();

            string nombreSiguienteNivel = categoriaSiguiente?.NomCat;

            var obras = await _context.Obras
                .Where(o => o.CedDoc == cedula && o.FechaPublicacion >= fechaInicio)
                .CountAsync();

            var cursos = await _context.CursosCapacitacions
                .Where(c => c.CedDoc == cedula && c.FechaCurso >= fechaInicio)
                .ToListAsync();

            var totalCursos = cursos.Count;
            var horasCursos = cursos.Sum(c => c.Horas);

            var investigaciones = await _context.Investigaciones
            .Where(i => i.CedDoc == cedula && i.FechaFin >= fechaInicio)
             .ToListAsync();

            var mesesInvestigacion = investigaciones.Sum(i => i.DuracionMeses);

            var evaluaciones = await _context.Evaluaciones
                .Where(e => e.CedDoc == cedula && e.FechaEvaluacion >= fechaInicio)
                .ToListAsync();

            var totalEvaluaciones = evaluaciones.Count;
            var promedioEvaluacion = evaluaciones.Any() ? evaluaciones.Average(e => (double)e.Resultado) : 0;

            var docente = await _context.Docentes
                .Where(d => d.CedDoc == cedula)
                .Select(d => d.FechaContratacion)
                .FirstOrDefaultAsync();

            // Convert DateOnly to DateTime for subtraction
            var aniosServicio = Math.Round((DateTime.Now - fechaInicio.ToDateTime(TimeOnly.MinValue)).TotalDays / 365, 2);

            return Ok(new
            {
                FechaBase = fechaInicio.ToString("yyyy-MM-dd"),
                TotalObras = obras,
                TotalCursos = totalCursos,
                HorasCursos = horasCursos,
                MesesInvestigacion = mesesInvestigacion,
                TotalEvaluaciones = totalEvaluaciones,
                PromedioEvaluacion = Math.Round(promedioEvaluacion, 2),
                AniosServicio = aniosServicio,
                Nivel = nombreNivelActual,
                categoriaSiguiente = nombreSiguienteNivel,
                Cat1 = idNivelActual,
                Cat2 = categoriaSiguiente?.IdCat ?? ""
            });
        }
    }

}

