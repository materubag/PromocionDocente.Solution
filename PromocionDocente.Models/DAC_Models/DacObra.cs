using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.DAC_Models;

public partial class DacObra
{
    public int IdObra { get; set; }

    public string CedDoc { get; set; } = null!;

    public string TipoObra { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public DateOnly FechaPublicacion { get; set; }

    public string? DoiUrl { get; set; }

    public string? AreaConocimiento { get; set; }

    public string? Observaciones { get; set; }

    public byte[]? PdfProduccion { get; set; }
}
