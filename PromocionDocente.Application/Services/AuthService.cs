using PromocionDocente.Application.DTOs;
using PromocionDocente.Domain.Interfaces;

namespace PromocionDocente.Application.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _repository;

        public AuthService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<Login_Request?> LoginAsync(Login dto)
        {
            var usuario = await _repository.LoginAsync(dto.Correo, dto.Contrasena);
            if (usuario == null) return null;

            return new Login_Request
            {
                Cedula = usuario.Cedula,
                Nombre1 = usuario.Nombre1,
                Nombre2 = usuario.Nombre2,
                Apellido1 = usuario.Apellido1,
                Apellido2 = usuario.Apellido2,
                Facultad = usuario.Facultad,
                Correo = usuario.Correo,
                Rol = usuario.Rol
            };
        }
    }
}
