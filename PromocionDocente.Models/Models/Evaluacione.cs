using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.Models;

public partial class Evaluacione
{
    public int IdEvaluacion { get; set; }

    public string CedDoc { get; set; } = null!;

    public string PeriodoEvaluado { get; set; } = null!;

    public string TipoEvaluacion { get; set; } = null!;

    public DateOnly FechaEvaluacion { get; set; }

    public decimal Resultado { get; set; }

    public string? Observaciones { get; set; }

    public byte[]? PdfEvaluacion { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observacion { get; set; }

    public virtual Docente CedDocNavigation { get; set; } = null!;
}
