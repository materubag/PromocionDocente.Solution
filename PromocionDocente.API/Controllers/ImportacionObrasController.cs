using Microsoft.AspNetCore.Mvc;
using PromocionDocente.Application.Interfaces;
using System.Threading.Tasks;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacionObrasController : ControllerBase
    {
        private readonly IObraImportService _importService;

        public ImportacionObrasController(IObraImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        public async Task<IActionResult> Importar()
        {
            await _importService.ImportarObrasDesdeDACAsync();
            return Ok(new { mensaje = "Obras importadas correctamente" });
        }
    }
}
