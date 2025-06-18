using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class ResultadosCapacitacion
{
    public int Id { get; set; }

    public int EmpleadoId { get; set; }

    public int CapacitacionId { get; set; }

    public decimal Nota { get; set; }

    public bool Aprobado { get; set; }

    public DateTime FechaEvaluacion { get; set; }

    public virtual Capacitaciones Capacitacion { get; set; } = null!;

    public virtual ICollection<Certificados> Certificados { get; set; } = new List<Certificados>();

    public virtual Empleados Empleado { get; set; } = null!;
}
