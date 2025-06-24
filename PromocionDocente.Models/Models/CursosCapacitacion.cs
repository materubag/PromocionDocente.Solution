using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.Models;

public partial class CursosCapacitacion
{
    public int IdCurso { get; set; }

    public string CedDoc { get; set; } = null!;

    public string NombreCurso { get; set; } = null!;

    public DateOnly FechaCurso { get; set; }

    public int Horas { get; set; }

    public byte[]? PdfCurso { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observacion { get; set; }

    public virtual Docente CedDocNavigation { get; set; } = null!;
}
