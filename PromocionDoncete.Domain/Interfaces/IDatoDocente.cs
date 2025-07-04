
using PromocionDocente.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Domain.Interfaces
{
    public interface IDatoDocente
    {
        Task AddAsync(Docente log);
        Task<bool> ExisteDocenteAsync(string id);
    }
}
