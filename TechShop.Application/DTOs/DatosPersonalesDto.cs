using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Application.DTOs
{
    public class DatosPersonalesDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public string Email { get; set; } = "";
        public DateOnly? FechaNacimiento { get; set; }
        public bool IsActive { get; set; }
    }
}
