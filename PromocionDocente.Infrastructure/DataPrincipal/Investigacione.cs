using System;
using System.Collections.Generic;

namespace PromocionDocente.Infrastructure.DataPrincipal;

public partial class Investigacione
{
    public int IdInvestigacion { get; set; }

    public string CedDoc { get; set; } = null!;

    public string TituloInvestigacion { get; set; } = null!;

    public int DuracionMeses { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public byte[]? ArchivoPdf { get; set; }

    public string TipoInvestigacion { get; set; } = null!;

    public string CampoAplicacion { get; set; } = null!;

    public virtual Docente CedDocNavigation { get; set; } = null!;
}
