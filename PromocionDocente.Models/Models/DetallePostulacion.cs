using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.Models;

public partial class DetallePostulacion
{
    public int IdDet { get; set; }

    public int IdPos { get; set; }

    public string Observacion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? TablaOrigen { get; set; }

    public string? IdOrigen { get; set; }

    public virtual Postulacione IdPosNavigation { get; set; } = null!;
}
