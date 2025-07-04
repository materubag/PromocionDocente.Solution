using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs.Update
{
    public class EvaluacionUpdateDto
    {
        public string PeriodoEvaluado { get; set; } = "";
        public string TipoEvaluacion { get; set; } = "";
        public DateTime FechaEvaluacion { get; set; }   // <-- USA DateTime aquí
        public decimal Resultado { get; set; }
        public string? Observaciones { get; set; }
        public byte[]? PdfEvaluacion { get; set; }
    }

}
