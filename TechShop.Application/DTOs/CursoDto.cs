﻿namespace TechShop.Web.Models
{
    public class CursoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}