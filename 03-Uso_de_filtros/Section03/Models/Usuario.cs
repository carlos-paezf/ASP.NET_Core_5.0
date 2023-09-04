using System;
using System.Collections.Generic;

namespace Section03.Models;

public partial class Usuario
{
    public int Iidusuario { get; set; }

    public int Iidtipousuario { get; set; }

    public string? Nombreusuario { get; set; }

    public string? Contraseña { get; set; }

    public int? Bhabilitado { get; set; }

    public int? Iidpersona { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<HistorialCitum> HistorialCita { get; set; } = new List<HistorialCitum>();

    public virtual Persona? IidpersonaNavigation { get; set; }

    public virtual TipoUsuario IidtipousuarioNavigation { get; set; } = null!;

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
