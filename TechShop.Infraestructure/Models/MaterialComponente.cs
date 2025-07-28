using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class MaterialComponente
{
    public int Id { get; set; }

    public int? SeccionId { get; set; }

    public string? Contenido { get; set; }

    public string? Tipo { get; set; }

    public int? Posicion { get; set; }

    public virtual SeccionCurso? Seccion { get; set; }
}
