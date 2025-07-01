using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs
{

    public class EvaluacionDto
    {
        public string CedDoc { get; set; } = string.Empty;
        public string PeriodoEvaluado { get; set; } = string.Empty;
        public string TipoEvaluacion { get; set; } = string.Empty;
        public DateTime FechaEvaluacion { get; set; }
        public decimal Resultado { get; set; }
        public string? Observaciones { get; set; }
        public byte[]? PdfEvaluacion { get; set; }
        public string Estado { get; set; } = "PENDIENTE";
        public string? Observacion { get; set; }
    }

}
