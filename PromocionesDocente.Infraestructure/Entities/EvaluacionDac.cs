using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Infrastructure.Entities
{
    public class EvaluacionDac
    {
        public int IdEvaluacion { get; set; }
        public string NumeroResolucion { get; set; } = string.Empty;
        public DateTime FechaEvaluacion { get; set; }
        public decimal Calificacion { get; set; }
        public byte[]? PdfEvaluacion { get; set; }
        public string CedulaDocente { get; set; } = string.Empty;
    }
}
