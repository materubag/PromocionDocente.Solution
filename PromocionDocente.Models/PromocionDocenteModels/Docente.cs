using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.PromocionDocenteModels;

public partial class Docente
{
    public string CedDoc { get; set; } = null!;

<<<<<<< HEAD
    public string NomDoc { get; set; } = null!;

    public string ApeDoc { get; set; } = null!;

    public string TelDoc { get; set; } = null!;
=======
    public string? TelDoc { get; set; }
>>>>>>> fcd84467b8d968067540d8a59f5a11007c1a5966

    public DateOnly FecIng { get; set; }

    public DateOnly FecNac { get; set; }

    public string IdFac { get; set; } = null!;

    public string? NivelDocente { get; set; }

<<<<<<< HEAD
    public DateOnly FechaContratacion { get; set; }
=======
    public DateOnly? FechaContratacion { get; set; }
>>>>>>> fcd84467b8d968067540d8a59f5a11007c1a5966

    public DateOnly? FechaUltimoAscenso { get; set; }

    public byte[]? PdfContrato { get; set; }

    public string? EstadoContrato { get; set; }

<<<<<<< HEAD
=======
    public string Nom1Doc { get; set; } = null!;

    public string Nom2Doc { get; set; } = null!;

    public string Ape1Doc { get; set; } = null!;

    public string Ape2Doc { get; set; } = null!;

>>>>>>> fcd84467b8d968067540d8a59f5a11007c1a5966
    public virtual ICollection<CursosCapacitacion> CursosCapacitacions { get; set; } = new List<CursosCapacitacion>();

    public virtual ICollection<Evaluacione> Evaluaciones { get; set; } = new List<Evaluacione>();

    public virtual ICollection<HistorialDocente> HistorialDocentes { get; set; } = new List<HistorialDocente>();

    public virtual Facultade IdFacNavigation { get; set; } = null!;

    public virtual ICollection<Investigacione> Investigaciones { get; set; } = new List<Investigacione>();

    public virtual ICollection<Obra> Obras { get; set; } = new List<Obra>();

    public virtual ICollection<Postulacione> Postulaciones { get; set; } = new List<Postulacione>();
}
