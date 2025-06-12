
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Application.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthService _authService;

        public UserController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized("Correo o contraseña inválidos");

            return Ok(result);
        }
    }
}
