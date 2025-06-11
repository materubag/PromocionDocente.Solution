using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.PromocionDocenteModels;

public partial class Categoria
{
    public string IdCat { get; set; } = null!;

    public string NomCat { get; set; } = null!;

    public string DesCat { get; set; } = null!;

    public int NivCat { get; set; }

    public virtual ICollection<HistorialDocente> HistorialDocentes { get; set; } = new List<HistorialDocente>();

    public virtual ICollection<Postulacione> Postulaciones { get; set; } = new List<Postulacione>();
}
