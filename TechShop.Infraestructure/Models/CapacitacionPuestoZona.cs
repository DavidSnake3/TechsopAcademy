using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class CapacitacionPuestoZona
{
    public int Id { get; set; }

    public int CapacitacionId { get; set; }

    public int PuestoId { get; set; }

    public int ZonaId { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public virtual Capacitaciones Capacitacion { get; set; } = null!;
}
