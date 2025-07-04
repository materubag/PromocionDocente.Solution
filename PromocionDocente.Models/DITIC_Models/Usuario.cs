using System;
using System.Collections.Generic;

namespace PromocionDocente.Models.DITIC_Models;

public partial class Usuario
{
    public string Cedula { get; set; } = null!;

    public string Nombre1 { get; set; } = null!;

    public string? Nombre2 { get; set; }

    public string Apellido1 { get; set; } = null!;

    public string? Apellido2 { get; set; }

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? Facultad { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public int? Nivel { get; set; }

    public string Rol { get; set; } = null!;

    public string? Telefono { get; set; }

    public virtual ICollection<CursosCapacitacion> CursosCapacitacions { get; set; } = new List<CursosCapacitacion>();
}
