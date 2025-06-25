using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs
{
    public class ReporteRechazadosDto
    {
        public string CedulaDocente { get; set; }
        public string NombreDocente { get; set; }
        public string CategoriaActual { get; set; }
        public DateOnly? UltimaFechaHistorial { get; set; }
        public List<ObraRechazadaDto> ObrasRechazadas { get; set; } = new List<ObraRechazadaDto>();
        public List<InvestigacionRechazadaDto> InvestigacionesRechazadas { get; set; } = new List<InvestigacionRechazadaDto>();
        public List<CursoRechazadoDto> CursosRechazados { get; set; } = new List<CursoRechazadoDto>();
        public List<EvaluacionRechazadaDto> EvaluacionesRechazadas { get; set; } = new List<EvaluacionRechazadaDto>();
        public int TotalRechazados { get; set; }
        public string FechaGeneracion { get; set; }
        public string UsuarioGeneracion { get; set; }
    }

    public class ObraRechazadaDto
    {
        public int IdObra { get; set; }
        public string Titulo { get; set; }
        public string TipoObra { get; set; }
        public DateOnly? FechaPublicacion { get; set; }
        public string Observacion { get; set; }
    }

    public class InvestigacionRechazadaDto
    {
        public int IdInvestigacion { get; set; }
        public string TituloInvestigacion { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string Observacion { get; set; }
    }

    public class CursoRechazadoDto
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public DateOnly? FechaCurso { get; set; }
        public int? Horas { get; set; }
        public string Observacion { get; set; }
    }

    public class EvaluacionRechazadaDto
    {
        public int IdEvaluacion { get; set; }
        public string TipoEvaluacion { get; set; }
        public string PeriodoEvaluado { get; set; }
        public decimal? Resultado { get; set; }
        public string Observacion { get; set; }
    }
}
