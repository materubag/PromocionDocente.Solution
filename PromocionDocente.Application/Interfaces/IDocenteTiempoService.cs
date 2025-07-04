using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromocionDocente.Domain.Entities;

namespace PromocionDocente.Application.Interfaces
{
    public interface IDocenteTiempoService
    {
        Task<DocenteTiempo?> ObtenerTiempoDocenteAsync(string cedula, DateTime? fechaCorte = null);
    }
}

