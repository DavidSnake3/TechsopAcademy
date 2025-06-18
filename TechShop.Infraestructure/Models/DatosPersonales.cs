using System;
using System.Collections.Generic;

namespace TechShop.Infraestructure.Models;

public partial class DatosPersonales
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly? FechaNacimiento { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Empleados> Empleados { get; set; } = new List<Empleados>();
}
