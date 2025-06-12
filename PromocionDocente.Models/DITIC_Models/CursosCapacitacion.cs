using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.DITIC_Models;

public partial class CursosCapacitacion
{
    public int IdCurso { get; set; }

    public string CedulaUsuario { get; set; } = null!;

    public string NombreCurso { get; set; } = null!;

    public DateOnly FechaCurso { get; set; }

    public int Horas { get; set; }

    public byte[]? PdfCurso { get; set; }

    public virtual Usuario CedulaUsuarioNavigation { get; set; } = null!;
}
