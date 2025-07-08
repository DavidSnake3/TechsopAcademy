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

        public IEnumerable<MaterialDto> Materiales { get; set; }
        public IEnumerable<PreguntaDto> Preguntas { get; set; }
    }

    public class MaterialDto
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; } = "";
        public string RutaArchivo { get; set; } = "";
        public string TipoArchivo { get; set; } = "";
        public DateTime FechaSubida { get; set; }
        public string? VideoUrl { get; set; }
    }

    public class PreguntaDto
    {
        public int Id { get; set; }
        public string TextoPregunta { get; set; } = "";
        public string TipoPregunta { get; set; } = "";
        public IEnumerable<OpcionRespuestaDto> Opciones { get; set; }
    }

    public class OpcionRespuestaDto
    {
        public int Id { get; set; }
        public string TextoOpcion { get; set; } = "";
        public bool EsCorrecta { get; set; }
    }
}
