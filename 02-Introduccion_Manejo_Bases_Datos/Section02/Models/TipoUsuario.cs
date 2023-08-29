using System;
using System.Collections.Generic;

namespace Section02.Models;

public partial class TipoUsuario
{
    public int Iidtipousuario { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<TipoUsuarioPagina> TipoUsuarioPaginas { get; set; } = new List<TipoUsuarioPagina>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
