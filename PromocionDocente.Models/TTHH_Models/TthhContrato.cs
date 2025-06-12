using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.TTHH_Models;

public partial class TthhContrato
{
    public int IdContrato { get; set; }

    public string CedDoc { get; set; } = null!;

    public string NombreDocente { get; set; } = null!;

    public string? NivelDocente { get; set; }

    public DateOnly FechaContratacion { get; set; }

    public DateOnly? FechaUltimoAscenso { get; set; }

    public byte[]? PdfContrato { get; set; }

    public string? EstadoContrato { get; set; }
}
