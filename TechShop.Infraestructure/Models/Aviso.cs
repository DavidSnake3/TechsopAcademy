using System;
using System.ComponentModel.DataAnnotations;

namespace TechShop.Infraestructure.Models
{
    public class Aviso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El texto del aviso es requerido")]
        [StringLength(1000, ErrorMessage = "El aviso no puede exceder 1000 caracteres")]
        public string Texto { get; set; } = string.Empty;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "La fecha de expiración es requerida")]
        public DateTime FechaExpiracion { get; set; }

        public int CreadoPorEmpleadoId { get; set; }

        public string? CreadoPorNombre { get; set; }

        public bool EstaActivo => DateTime.UtcNow <= FechaExpiracion;
    }
}