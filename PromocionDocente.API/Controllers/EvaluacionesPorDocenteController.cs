using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Domain.Entities;

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

            return Ok(evaluaciones);
        }
    }
}
