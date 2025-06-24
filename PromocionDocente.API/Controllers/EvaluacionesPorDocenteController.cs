using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Utils;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/evaluaciones")]
    public class EvaluacionesPorDocenteController : ControllerBase
    {
        private readonly PromocionDocenteDbContext _context;

        public EvaluacionesPorDocenteController(PromocionDocenteDbContext context)
        {
            _context = context;
        }

        [HttpGet("{cedula}")]
        public async Task<IActionResult> GetEvaluacionesPorCedula(string cedula)
        {
            var evaluaciones = await _context.Evaluaciones
                .Where(e => e.CedulaDocente == cedula)
                .ToListAsync();

            if (evaluaciones == null || evaluaciones.Count == 0)
                return NotFound();

            var resultado = evaluaciones.Select(e => new
            {
                e.IdEvaluacion,
                e.FechaEvaluacion,
                e.Calificacion,
                e.CedulaDocente,
                e.TipoEvaluacion,
                e.PeriodoEvaluacion,
                PdfEvaluacion = e.PdfEvaluacion != null
                    ? PdfHelper.GuardarBinarioComoPdf(e.PdfEvaluacion, e.CedulaDocente, $"{e.TipoEvaluacion}_{e.PeriodoEvaluacion}", "Evaluaciones")
                    : ""
            }).ToList();

            return Ok(resultado);
        }
    }
}
