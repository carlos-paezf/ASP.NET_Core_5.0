using System;
using System.Collections.Generic;

namespace Section03.Models;

public partial class Boton
{
    public int Iidboton { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<TipoUsuarioPaginaBoton> TipoUsuarioPaginaBotons { get; set; } = new List<TipoUsuarioPaginaBoton>();
}
