using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Domain.Entities;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/obras")]
    public class ObrasPorDocenteController : ControllerBase
    {
        private readonly PromocionDocenteDbContext _context;

        public ObrasPorDocenteController(PromocionDocenteDbContext context)
        {
            _context = context;
        }

        [HttpGet("{cedula}")]
        public async Task<IActionResult> GetObrasPorCedula(string cedula)
        {
            var obras = await _context.Obras
                .Where(o => o.CedulaDocente == cedula)
                .ToListAsync();
            
            return Ok(obras);
        }
    }
}
