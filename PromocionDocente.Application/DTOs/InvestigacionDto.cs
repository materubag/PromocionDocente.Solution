namespace PromocionDocente.Application.DTOs
{
    public class InvestigacionDto
    {
        public int IdInvestigacion { get; set; }
        public string CedulaDocente { get; set; } = string.Empty;
        public string TituloInvestigacion { get; set; } = string.Empty;
        public int DuracionMeses { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public byte[]? ArchivoPdf { get; set; }
        public string TipoInvestigacion { get; set; } = string.Empty;
        public string CampoAplicacion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Observacion { get; set; }
    }
}
