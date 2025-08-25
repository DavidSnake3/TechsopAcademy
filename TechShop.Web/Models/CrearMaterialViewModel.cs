namespace TechShop.Web.Models
{
    public class ComponenteMaterial
    {
        public string Tipo { get; set; } // "Texto", "Video", "Archivo", "Titulo"
        public string Contenido { get; set; } // texto o URL
        public IFormFile? Archivo { get; set; } // si es tipo Archivo
    }

    public class SeccionMaterial
    {
        public string TituloSeccion { get; set; }
        public List<ComponenteMaterial> Componentes { get; set; } = new();
    }

    public class CrearMaterialViewModel
    {
        public List<SeccionMaterial> Secciones { get; set; } = new();
    }
}
