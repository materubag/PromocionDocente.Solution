using Microsoft.AspNetCore.Mvc;
using PromocionDocente.Application.Interfaces;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/importar/historial")]
    public class HistorialDocenteImportController : ControllerBase
    {
        private readonly IHistorialDocenteImportService _service;

        public HistorialDocenteImportController(IHistorialDocenteImportService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Importar()
        {
            var resultado = await _service.ImportarHistorialDocenteAsync();
            return Ok(new { mensaje = resultado });
        }
    }
}
