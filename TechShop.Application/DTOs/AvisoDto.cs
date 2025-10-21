using System;
using System.ComponentModel.DataAnnotations;

namespace TechShop.Application.DTOs
{
    public class AvisoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El texto del aviso es requerido")]
        [StringLength(1000, ErrorMessage = "El aviso no puede exceder 1000 caracteres")]
        public string Texto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La duración es requerida")]
        [Range(1, 365, ErrorMessage = "La duración debe ser entre 1 y 365 días")]
        public int DuracionDias { get; set; } = 7;

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string? CreadoPorNombre { get; set; }
        public bool EstaActivo { get; set; }
    }
}