namespace TechShop.Web.Models
{
    public class DashboardViewModel
    {
        public List<CursoDto> Completados { get; set; } = new();
        public List<CursoDto> EnProceso { get; set; } = new();
        public List<CursoDto> Finalizados { get; set; } = new();
        public List<CursoDto> Disponibles { get; set; } = new();
        public List<Aviso> Avisos { get; set; } = new();
    }
}
