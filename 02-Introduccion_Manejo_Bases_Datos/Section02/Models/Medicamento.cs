using System;
using System.Collections.Generic;

namespace Section02.Models;

public partial class Medicamento
{
    public int Iidmedicamento { get; set; }

    public string? Nombre { get; set; }

    public string? Concentracion { get; set; }

    public int? Iidformafarmaceutica { get; set; }

    public decimal? Precio { get; set; }

    public int? Stock { get; set; }

    public string? Presentacion { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<CitaMedicamento> CitaMedicamentos { get; set; } = new List<CitaMedicamento>();

    public virtual FormaFarmaceutica? IidformafarmaceuticaNavigation { get; set; }
}
