using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Application.DTOs
{
    public class CursoDetailDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Codigo { get; set; }
        public string? DescripcionCorta { get; set; }
        public string? DescripcionLarga { get; set; }

        /// <summary>Corresponde directamente al campo SQL time(7)</summary>
        public TimeOnly? DuracionHoras { get; set; }

        public string? Dificultad { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public byte[]? Foto { get; set; }

        public string? Departamento { get; set; }

        public string? Puestos { get; set; }

        public string? Zonas { get; set; }

        public string? Usuario { get; set; }

        public IEnumerable<MaterialDto> Materiales { get; set; } = new List<MaterialDto>();
    }

    public class MaterialDto
    {
        public int Id { get; set; }
        public string? NombreArchivo { get; set; } 
        public string? RutaArchivo { get; set; } 
        public string? TipoArchivo { get; set; } 
        public DateTime FechaSubida { get; set; }
        public string? VideoUrl { get; set; }
    }
}
