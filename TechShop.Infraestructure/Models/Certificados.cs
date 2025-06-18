using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class Certificados
{
    public int Id { get; set; }

    public int ResultadoId { get; set; }

    public string RutaArchivo { get; set; } = null!;

    public DateTime FechaGeneracion { get; set; }

    public virtual ResultadosCapacitacion Resultado { get; set; } = null!;
}
