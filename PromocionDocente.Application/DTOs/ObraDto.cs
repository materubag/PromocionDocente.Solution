using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs
{
    public class ObraDto
    {
        public string CedDoc { get; set; } = string.Empty;
        public string TipoObra { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; }
        public string? DoiUrl { get; set; }
        public string? AreaConocimiento { get; set; }
        public string? Observaciones { get; set; }
        public byte[]? PdfProduccion { get; set; }
        public string Estado { get; set; } = "PENDIENTE";
        public string? Observacion { get; set; }
    }

}
