using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.Models;

public partial class DetallePostulacion
{
    public int IdDet { get; set; }

    public int IdPos { get; set; }

    public string TipoIncumplimiento { get; set; } = null!;

    public string Detalle { get; set; } = null!;

    public bool? Rechazado { get; set; }

    public string? TablaOrigen { get; set; }

    public string? IdOrigen { get; set; }

    public virtual Postulacione IdPosNavigation { get; set; } = null!;
}
