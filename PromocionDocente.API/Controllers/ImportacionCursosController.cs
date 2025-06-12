using Microsoft.AspNetCore.Mvc;
using PromocionDocente.Application.Interfaces;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacionCursosController : ControllerBase
    {
        private readonly ICursoImportService _importService;

        public ImportacionCursosController(ICursoImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        public async Task<IActionResult> Importar()
        {
            await _importService.ImportarCursosDesdeDACAsync();
            return Ok(new { mensaje = "Cursos importados correctamente" });
        }
    }
}

