using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs
{
    
        public class EvaluacionDto
        {
            public int IdEvaluacion { get; set; }
            public DateTime FechaEvaluacion { get; set; }
            public decimal Calificacion { get; set; }
            public byte[]? PdfEvaluacion { get; set; }
            public string CedulaDocente { get; set; } = string.Empty;
            public string TipoEvaluacion { get; set; } = string.Empty;
            public string PeriodoEvaluacion { get; set; } = string.Empty;
        }
    }
