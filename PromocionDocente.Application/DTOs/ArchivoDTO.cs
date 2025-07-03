using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;


namespace PromocionDocente.Application.DTOs
{
    public class ArchivoDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string Encargado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string RutaArchivo { get; set; } = string.Empty;
        public byte[]? ContenidoArchivo { get; set; } // Para WebAssembly

    }
}


