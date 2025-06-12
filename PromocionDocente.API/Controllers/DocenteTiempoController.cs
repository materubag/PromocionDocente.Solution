using Microsoft.AspNetCore.Mvc;
using PromocionDocente.Application.Interfaces;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/docentes/tiempo")]
    public class DocenteTiempoController : ControllerBase
    {
        private readonly IDocenteTiempoService _service;

        public DocenteTiempoController(IDocenteTiempoService service)
        {
            _service = service;
        }

        [HttpGet("{cedula}")]
        public async Task<IActionResult> GetTiempoDocente(string cedula)
        {
            var resultado = await _service.ObtenerTiempoDocenteAsync(cedula);
            if (resultado == null) return NotFound("Docente no encontrado.");
            return Ok(resultado);
        }
    }
}
