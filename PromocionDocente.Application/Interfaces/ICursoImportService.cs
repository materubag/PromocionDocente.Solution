using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.Interfaces
{
    public interface ICursoImportService
    {
        Task ImportarCursosDesdeDACAsync();
    }
}
