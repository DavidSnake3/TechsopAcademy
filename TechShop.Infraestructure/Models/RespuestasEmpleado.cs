using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class RespuestasEmpleado
{
    public int Id { get; set; }

    public int EmpleadoId { get; set; }

    public int PreguntaId { get; set; }

    public int? OpcionRespuestaId { get; set; }

    public string? RespuestaTexto { get; set; }

    public DateTime FechaRespuesta { get; set; }

    public virtual Empleados Empleado { get; set; } = null!;

    public virtual OpcionesRespuesta? OpcionRespuesta { get; set; }

    public virtual Preguntas Pregunta { get; set; } = null!;
}
