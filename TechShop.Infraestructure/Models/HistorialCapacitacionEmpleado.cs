using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class HistorialCapacitacionEmpleado
{
    public int Id { get; set; }

    public int EmpleadoId { get; set; }

    public int CapacitacionId { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public DateTime? FechaCompletado { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Capacitaciones Capacitacion { get; set; } = null!;
}
