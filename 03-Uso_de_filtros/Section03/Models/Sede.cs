using System;
using System.Collections.Generic;

namespace Section03.Models;

public partial class Sede
{
    public int Iidsede { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
