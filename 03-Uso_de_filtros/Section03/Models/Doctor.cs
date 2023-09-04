using System;
using System.Collections.Generic;

namespace Section03.Models;

public partial class Doctor
{
    public int Iiddoctor { get; set; }

    public int? Iidsede { get; set; }

    public int? Iidespecialidad { get; set; }

    public decimal? Sueldo { get; set; }

    public DateTime? Fechacontrato { get; set; }

    public int? Iidpersona { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Especialidad? IidespecialidadNavigation { get; set; }

    public virtual Persona? IidpersonaNavigation { get; set; }

    public virtual Sede? IidsedeNavigation { get; set; }
}
