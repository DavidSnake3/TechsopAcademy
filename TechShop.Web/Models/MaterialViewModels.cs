using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace TechShop.Web.Models
{
    public class ComponenteMaterialViewModel
    {
        public int Id { get; set; }
        public int SeccionId { get; set; }
        public string Tipo { get; set; }         // "Texto", "Video", "Archivo", "Titulo"
        public string Contenido { get; set; }    // texto o URL
        public int Posicion { get; set; }

        public IFormFile Archivo { get; set; }
    }

    public class SeccionMaterialViewModel
    {
        public int Id { get; set; }
        public int CapacitacionId { get; set; }
        public string Titulo { get; set; }
        public int Posicion { get; set; }

        public List<ComponenteMaterialViewModel> Componentes { get; set; }
            = new List<ComponenteMaterialViewModel>();
    }

    public class MaterialCursoViewModel
    {
        public int CapacitacionId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string DescripcionCorta { get; set; }

        public bool EstadoProceso { get; set; }

        public List<SeccionMaterialViewModel> Secciones { get; set; }
            = new List<SeccionMaterialViewModel>();
    }
}