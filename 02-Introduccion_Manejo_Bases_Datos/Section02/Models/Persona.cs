using System;
using System.Collections.Generic;

namespace Section02.Models;

public partial class Persona
{
    public int Iidpersona { get; set; }

    public string? Nombre { get; set; }

    public string? Appaterno { get; set; }

    public string? Apmaterno { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public string? Telefonofijo { get; set; }

    public string? Telefonocelular { get; set; }

    public DateTime? Fechanacimiento { get; set; }

    public int? Iidsexo { get; set; }

    public int? Bdoctor { get; set; }

    public int? Bpaciente { get; set; }

    public int? Bhabilitado { get; set; }

    public string? Foto { get; set; }

    public int? Btieneusuario { get; set; }

    public int? Iidusuario { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual Sexo? IidsexoNavigation { get; set; }

    public virtual Usuario? IidusuarioNavigation { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
