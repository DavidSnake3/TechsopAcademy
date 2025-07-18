using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class MaterialCurso
{
    public List<SeccionMaterial> Secciones { get; set; } = new();

    public virtual Capacitaciones Capacitacion { get; set; } = null!;
}

public class SeccionMaterial
{
    public int Id { get; set; }

    public int CapacitacionId { get; set; }

    public string Titulo { get; set; } = null!;

    public int Posicion { get; set; }

    public List<ComponenteMaterial> Componentes { get; set; } = new();
}

public class ComponenteMaterial
{

    public int Id { get; set; }
    public int SeccionId { get; set; }
    public string Tipo { get; set; } // "Texto", "Video", "Archivo", "Titulo"
    public string Contenido { get; set; } // texto o URL
    public int posicion { get; set; }
}

