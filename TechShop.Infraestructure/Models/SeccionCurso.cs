using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class SeccionCurso
{
    public int Id { get; set; }

    public int CursoId { get; set; }

    public string Titulo { get; set; } = null!;

    public int Posicion { get; set; }

    public virtual Capacitaciones Curso { get; set; } = null!;

    public virtual ICollection<MaterialComponente> MaterialComponente { get; set; } = new List<MaterialComponente>();
}
