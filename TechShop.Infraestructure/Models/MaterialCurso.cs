using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class MaterialCurso
{
    public int Id { get; set; }

    public int CapacitacionId { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string RutaArchivo { get; set; } = null!;

    public string TipoArchivo { get; set; } = null!;

    public DateTime FechaSubida { get; set; }

    public virtual Capacitaciones Capacitacion { get; set; } = null!;
}
