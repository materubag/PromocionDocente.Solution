using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.Models;

public partial class Postulacione
{
    public int IdPos { get; set; }

    public string CedDoc { get; set; } = null!;

    public string IdCat { get; set; } = null!;

    public DateOnly FecPos { get; set; }

    public string? EstPos { get; set; }

    public string? ObsPos { get; set; }

    public virtual Docente CedDocNavigation { get; set; } = null!;

    public virtual ICollection<DetallePostulacion> DetallePostulacions { get; set; } = new List<DetallePostulacion>();

    public virtual Categoria IdCatNavigation { get; set; } = null!;
}
