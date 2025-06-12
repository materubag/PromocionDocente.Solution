using PromocionDocente.Infrastructure.Data;

namespace PromocionDocente.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> LoginAsync(string correo, string contrasena);
    }
}
