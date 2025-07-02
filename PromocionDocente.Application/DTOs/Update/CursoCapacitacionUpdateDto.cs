using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs.Update
{
    public class CursoCapacitacionUpdateDto
    {
        public string NombreCurso { get; set; } = "";
        public DateTime FechaCurso { get; set; }
        public int Horas { get; set; }
        public byte[]? PdfCurso { get; set; }
        public string? Observacion { get; set; }
    }

}
