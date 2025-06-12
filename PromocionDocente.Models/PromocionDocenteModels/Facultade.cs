using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.PromocionDocenteModels;

public partial class Facultade
{
    public string IdFac { get; set; } = null!;

<<<<<<< HEAD
    public string NomFac { get; set; } = null!;

    public string UbiPreFac { get; set; } = null!;

    public string IdUni { get; set; } = null!;

    public virtual ICollection<Docente> Docentes { get; set; } = new List<Docente>();

    public virtual Universidade IdUniNavigation { get; set; } = null!;
=======
    public string? NomFac { get; set; }

    public string UbiPreFac { get; set; } = null!;

    public virtual ICollection<Docente> Docentes { get; set; } = new List<Docente>();
>>>>>>> fcd84467b8d968067540d8a59f5a11007c1a5966
}
