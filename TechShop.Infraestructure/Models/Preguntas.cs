using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class Preguntas
{
    public int Id { get; set; }

    public int CapacitacionId { get; set; }

    public string TextoPregunta { get; set; } = null!;

    public string TipoPregunta { get; set; } = null!;

    public virtual Capacitaciones Capacitacion { get; set; } = null!;

    public virtual ICollection<OpcionesRespuesta> OpcionesRespuesta { get; set; } = new List<OpcionesRespuesta>();

    public virtual ICollection<RespuestasEmpleado> RespuestasEmpleado { get; set; } = new List<RespuestasEmpleado>();
}
