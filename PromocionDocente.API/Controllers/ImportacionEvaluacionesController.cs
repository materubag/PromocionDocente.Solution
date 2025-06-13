using Microsoft.AspNetCore.Mvc;
using PromocionDocente.Application.Interfaces;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacionEvaluacionesController : ControllerBase
    {
        private readonly IEvaluacionImportService _importService;

        public ImportacionEvaluacionesController(IEvaluacionImportService importService)
        {
            _importService = importService;
        }

        [HttpGet("Ced/{cedula}")]
        public async Task<IActionResult> ImportarPorCedula(string cedula)
        {
            await _importService.ImportarEvaluacionesDesdeDACPorCedulaAsync(cedula);
            return Ok(new { mensaje = "Evaluaciones importadas correctamente" });
        }
    }
}
