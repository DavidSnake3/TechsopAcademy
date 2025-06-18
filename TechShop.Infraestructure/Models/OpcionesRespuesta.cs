using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class OpcionesRespuesta
{
    public int Id { get; set; }

    public int PreguntaId { get; set; }

    public string TextoOpcion { get; set; } = null!;

    public bool EsCorrecta { get; set; }

    public virtual Preguntas Pregunta { get; set; } = null!;

    public virtual ICollection<RespuestasEmpleado> RespuestasEmpleado { get; set; } = new List<RespuestasEmpleado>();
}
