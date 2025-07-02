using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.DTOs.Update;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Services;
using PromocionDocente.Infrastructure.Utils;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/evaluacion")]
    public class EvaluacionesPorDocenteController : ControllerBase
    {
        private readonly PromocionDocenteDbContext _context;
        private readonly EvaluacionService _evaluacionService;


        public EvaluacionesPorDocenteController(PromocionDocenteDbContext context,
            EvaluacionService evaluacionService)

        {
            _context = context;
            _evaluacionService = evaluacionService;
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
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEvaluacion(int id, [FromBody] EvaluacionUpdateDto dto)
        {
            await _evaluacionService.ActualizarEvaluacionAsync(id, dto);
            return NoContent();
        }


    }
}
