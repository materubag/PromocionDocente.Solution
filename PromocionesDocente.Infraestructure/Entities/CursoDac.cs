using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Infrastructure.Entities
{
    public class CursoDac
    {
        public int IdCurso { get; set; }
        public string CedulaDocente { get; set; } = string.Empty;
        public string NombreCurso { get; set; } = string.Empty;
        public DateTime FechaCurso { get; set; }
        public int Horas { get; set; }
        public byte[]? PdfCurso { get; set; }
    }
}

