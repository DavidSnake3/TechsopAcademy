using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class Empleados
{
    public int Id { get; set; }

    public int DatosPersonalesId { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public string Estado { get; set; } = null!;

    public virtual DatosPersonales DatosPersonales { get; set; } = null!;

    public virtual ICollection<HistorialCapacitacionEmpleado> HistorialCapacitacionEmpleado { get; set; } = new List<HistorialCapacitacionEmpleado>();

    public virtual ICollection<RespuestasEmpleado> RespuestasEmpleado { get; set; } = new List<RespuestasEmpleado>();

    public virtual ICollection<ResultadosCapacitacion> ResultadosCapacitacion { get; set; } = new List<ResultadosCapacitacion>();
}
