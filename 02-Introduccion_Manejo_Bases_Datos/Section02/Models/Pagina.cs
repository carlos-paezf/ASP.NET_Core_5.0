using System;
using System.Collections.Generic;

namespace Section02.Models;

public partial class Pagina
{
    public int Iidpagina { get; set; }

    public string? Mensaje { get; set; }

    public string? Accion { get; set; }

    public string? Controlador { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<TipoUsuarioPagina> TipoUsuarioPaginas { get; set; } = new List<TipoUsuarioPagina>();
}
