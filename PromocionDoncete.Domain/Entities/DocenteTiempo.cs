using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Domain.Entities
{
    public class DocenteTiempo
    {
        public string Cedula { get; set; } = string.Empty;
        public int Anios { get; set; }
        public int Meses { get; set; }
        public int Dias { get; set; }
    }
}
