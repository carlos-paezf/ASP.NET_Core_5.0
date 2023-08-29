using System;
using System.Collections.Generic;

namespace Section02.Models;

public partial class Especialidad
{
    public int Iidespecialidad { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
