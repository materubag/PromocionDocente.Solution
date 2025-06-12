
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Domain.Interfaces;
using PromocionDocente.Infrastructure.Data;

namespace Repository
{
    public class Usuario_Repository : IUsuarioRepository
    {
        private readonly DiticContext _context;
        public Usuario_Repository(DiticContext context)
        {
            _context = context;
        }


        public Task<Usuario?> LoginAsync(string correo, string contrasena)
        {
            return _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasena == contrasena);
        }
    }
}
