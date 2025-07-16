using System.ComponentModel.DataAnnotations;
using TechShop.Application.DTOs;

namespace TechShop.Web.Models
{
    public class CrearCursoConfigModel
    {
        public int Id { get; set; }
        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Codigo { get; set; }//Codigo personalizado

        [Required]
        public string? DescripcionCorta { get; set; }

        public string? DescripcionLarga { get; set; }

        public byte[] Foto { get; set; }

        [Required]
        [Range(1, 300)]
        public TimeOnly? DuracionHoras { get; set; }

        [Required]
        public string? Dificultad { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string? Usuario { get; set; } //El usuario que se guardara como evidencia de quien hizo el curso

        [Display(Name = "Departamentos")]
        public List<string> Departamentos { get; set; } = new();

        [Display(Name = "Roles")]
        public List<string> Roles { get; set; } = new();
        [Display(Name = "Puestos")]
        public List<string> Puestos { get; set; } = new();

        [Display(Name = "Zonas")]
        public List<string> Zonas { get; set; } = new();

        public IEnumerable<MaterialDto> Materiales { get; set; }
    }
}
