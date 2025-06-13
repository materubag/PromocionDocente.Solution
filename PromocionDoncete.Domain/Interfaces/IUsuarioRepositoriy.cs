using PromocionDocente.Models.DITIC_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> LoginAsync(string correo, string contrasena);
    }
}
