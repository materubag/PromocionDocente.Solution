public class ObraDto
{
    public int IdObra { get; set; }
    public string CedulaDocente { get; set; }
    public string TipoObra { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaPublicacion { get; set; }
    public string? DoiUrl { get; set; }
    public string? AreaConocimiento { get; set; }
    public string? Observaciones { get; set; }
    public byte[]? PdfProduccion { get; set; }
}
