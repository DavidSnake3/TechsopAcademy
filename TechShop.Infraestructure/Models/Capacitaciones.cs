using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class Capacitaciones
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public bool Activo { get; set; } = true;

    public virtual ICollection<CapacitacionPuestoZona> CapacitacionPuestoZona { get; set; } = new List<CapacitacionPuestoZona>();

    public virtual ICollection<HistorialCapacitacionEmpleado> HistorialCapacitacionEmpleado { get; set; } = new List<HistorialCapacitacionEmpleado>();

    public virtual ICollection<MaterialCurso> MaterialCurso { get; set; } = new List<MaterialCurso>();

    public virtual ICollection<Preguntas> Preguntas { get; set; } = new List<Preguntas>();

    public virtual ICollection<ResultadosCapacitacion> ResultadosCapacitacion { get; set; } = new List<ResultadosCapacitacion>();
}
