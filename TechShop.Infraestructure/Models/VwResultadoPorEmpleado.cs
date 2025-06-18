using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class VwResultadoPorEmpleado
{
    public int EmpleadoId { get; set; }

    public int CapacitacionId { get; set; }

    public int? TotalPreguntas { get; set; }

    public int? Correctas { get; set; }

    public decimal? NotaPorcentaje { get; set; }
}
