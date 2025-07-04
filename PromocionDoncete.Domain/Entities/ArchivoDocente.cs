using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Domain.Entities
{
    public class ArchivoDocente
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Encargado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string RutaArchivo { get; set; } = string.Empty;
    }
}

