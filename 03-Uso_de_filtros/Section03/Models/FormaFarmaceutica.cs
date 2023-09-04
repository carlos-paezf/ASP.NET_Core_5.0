using System;
using System.Collections.Generic;

namespace Section03.Models;

public partial class FormaFarmaceutica
{
    public int Iidformafarmaceutica { get; set; }

    public string? Nombre { get; set; }

    public int? Bhabilitado { get; set; }

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
}
