using System;
using System.Collections.Generic;

namespace Section03.Models;

public partial class EstadoCitum
{
    public int Iidestado { get; set; }

    public string? Vnombre { get; set; }

    public string? Vdescripcion { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<HistorialCitum> HistorialCita { get; set; } = new List<HistorialCitum>();
}
