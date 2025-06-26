using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromocionDocente.Application.DTOs;

namespace PromocionDocente.Application.Interfaces
{
    public interface IArchivoService
    {
        Task<bool> AgregarArchivoAsync(ArchivoDTO dto);
        Task<bool> ActualizarArchivoAsync(int id, ArchivoDTO dto);
        Task<ArchivoDTO?> ObtenerArchivoPorIdAsync(int id);


    }
}


