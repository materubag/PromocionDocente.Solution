using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs.Update
{
    public class InvestigacionUpdateDto
    {
        public string TituloInvestigacion { get; set; } = "";
        public int DuracionMeses { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public byte[]? ArchivoPdf { get; set; }
        public string TipoInvestigacion { get; set; } = "";
        public string CampoAplicacion { get; set; } = "";
        public string? Observacion { get; set; }
    }

}
