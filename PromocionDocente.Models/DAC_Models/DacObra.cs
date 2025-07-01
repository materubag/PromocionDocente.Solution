using System.ComponentModel.DataAnnotations.Schema;

namespace PromocionDocente.Models.DAC_Models;

[Table("OBRAS", Schema = "dbo")]
public partial class DacObra
{
    public int IdObra { get; set; }

    public string CedDoc { get; set; } = null!;

    public string TipoObra { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public DateTime FechaPublicacion { get; set; }  // Usa DateTime, no DateOnly

    public string? DoiUrl { get; set; }

    public string? AreaConocimiento { get; set; }

    public string? Observaciones { get; set; }

    public byte[]? PdfProduccion { get; set; }
}
