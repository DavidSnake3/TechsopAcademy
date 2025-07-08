using TechShop.Application.DTOs;

namespace TechShop.Web.Models
{
    public class CursoDetailViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string DescripcionCorta { get; set; }
        public string DescripcionLarga { get; set; }
        public TimeOnly? DuracionHoras { get; set; }
        public string Dificultad { get; set; }
        public DateTime FechaCreacion { get; set; }

        public IEnumerable<MaterialDto> Materiales { get; set; }
        public IEnumerable<PreguntaDto> Preguntas { get; set; }
    }
}
