using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.PromocionDocenteModels;

public partial class Universidade
{
    public string IdUni { get; set; } = null!;

    public string NomUni { get; set; } = null!;

    public string DirUni { get; set; } = null!;

    public string TelUni { get; set; } = null!;

    public virtual ICollection<Facultade> Facultades { get; set; } = new List<Facultade>();
}
