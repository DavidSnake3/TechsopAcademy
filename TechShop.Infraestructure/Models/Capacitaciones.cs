using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class Capacitaciones
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? DescripcionCorta { get; set; }

    public DateTime FechaCreacion { get; set; }

    public bool Activo { get; set; }

    public string? Codigo { get; set; }

    public string? DescripcionLarga { get; set; }

    public TimeOnly? DuracionHoras { get; set; }

    public string? Dificultad { get; set; }

    public byte[]? Foto { get; set; }

    public string? Departamento { get; set; }

    public string? Puestos { get; set; }

    public string? Zonas { get; set; }

    public string? Usuario { get; set; }

    public virtual ICollection<CapacitacionPuestoZona> CapacitacionPuestoZona { get; set; } = new List<CapacitacionPuestoZona>();

    public virtual ICollection<HistorialCapacitacionEmpleado> HistorialCapacitacionEmpleado { get; set; } = new List<HistorialCapacitacionEmpleado>();

    public virtual ICollection<MaterialCurso> MaterialCurso { get; set; } = new List<MaterialCurso>();

    public virtual ICollection<Preguntas> Preguntas { get; set; } = new List<Preguntas>();

    public virtual ICollection<ResultadosCapacitacion> ResultadosCapacitacion { get; set; } = new List<ResultadosCapacitacion>();
}
