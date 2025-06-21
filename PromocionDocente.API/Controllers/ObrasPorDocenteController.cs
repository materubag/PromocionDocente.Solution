using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Utils;

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

            var resultado = obras.Select(o =>
            {
                string pdfUrl = null;

                if (o.PdfProduccion != null)
                {
                    string rutaRelativa = PdfHelper.GuardarBinarioComoPdf(
                        o.PdfProduccion,
                        o.CedulaDocente,
                        o.Titulo,
                        "Obras"
                    );

                    pdfUrl = rutaRelativa;
                }
                return new
                {
                    o.IdObra,
                    o.CedulaDocente,
                    o.TipoObra,
                    o.Titulo,
                    o.FechaPublicacion,
                    o.DoiUrl,
                    o.AreaConocimiento,
                    o.Observaciones,
                    PdfUrl = pdfUrl
                };
            });

            return Ok(resultado);
        }
    }
}
