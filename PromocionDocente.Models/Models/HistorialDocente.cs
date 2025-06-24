using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.Models;

public partial class HistorialDocente
{
    public int IdHis { get; set; }

    public string CedDoc { get; set; } = null!;

    public string IdCat { get; set; } = null!;

    public DateOnly FecIni { get; set; }

    public DateOnly? FecFin { get; set; }

    public byte[]? DocumentoPdf { get; set; }

    public virtual Docente CedDocNavigation { get; set; } = null!;

    public virtual Categoria IdCatNavigation { get; set; } = null!;
}
