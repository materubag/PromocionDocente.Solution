public class ObraUpdateDto
{
    public string TipoObra { get; set; } = "";
    public string Titulo { get; set; } = "";
    public DateTime FechaPublicacion { get; set; }
    public string? DoiUrl { get; set; }
    public string? AreaConocimiento { get; set; }
    public string? Observaciones { get; set; }
    public byte[]? PdfProduccion { get; set; }
}
