using TechShop.Application.DTOs;

namespace TechShop.Web.Models
{
    public class DashboardViewModel
    {
        public List<CursoDetallelDto> Completados { get; set; } = new();
        public List<CursoDetallelDto> EnProceso { get; set; } = new();
        public List<CursoDetallelDto> Disponibles { get; set; } = new();
        public List<Aviso> Avisos { get; set; } = new();
    }
}
