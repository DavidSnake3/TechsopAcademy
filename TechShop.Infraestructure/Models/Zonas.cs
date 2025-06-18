using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class Zonas
{
    public int Id { get; set; }

    public string NombreZona { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<CapacitacionPuestoZona> CapacitacionPuestoZona { get; set; } = new List<CapacitacionPuestoZona>();
}
