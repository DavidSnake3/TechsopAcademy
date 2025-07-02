using System.ComponentModel.DataAnnotations;

namespace TechShop.Web.Models
{
    public class CrearCursoConfigModel
    {
            [Required]
            public string Nombre { get; set; }

            [Required]
            public string Codigo { get; set; } // Código personalizado del curso

            [Required]
            public string DescripcionCorta { get; set; }

            public string DescripcionLarga { get; set; }

            public IFormFile Foto { get; set; }

            [Required]
            [Range(1, 300)]
            public int DuracionHoras { get; set; }

            [Required]
            public string Dificultad { get; set; }

            public List<string> Departamentos { get; set; } = new();
            public List<string> Roles { get; set; } = new();
    }
}
