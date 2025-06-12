using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.DIDE_Models;

public partial class Dide
{
    public int IdInvestigacion { get; set; }

    public string CedulaDocente { get; set; } = null!;

    public string TituloInvestigacion { get; set; } = null!;

    public int DuracionMeses { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public byte[]? ArchivoPdf { get; set; }

    public string TipoInvestigacion { get; set; } = null!;

    public string CampoAplicacion { get; set; } = null!;
}
