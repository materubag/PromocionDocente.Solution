using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.Models;

public partial class Facultade
{
    public string IdFac { get; set; } = null!;

    public string? NomFac { get; set; }

    public string UbiPreFac { get; set; } = null!;

    public virtual ICollection<Docente> Docentes { get; set; } = new List<Docente>();
}
