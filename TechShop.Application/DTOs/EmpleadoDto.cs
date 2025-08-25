using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Application.DTOs
{
    public class EmpleadoDto
    {
        [JsonProperty("crfb9_codigo")]
        public string Crfb9_codigo { get; set; } = "";

        [JsonProperty("crfb9_contrasena")]
        public string Crfb9_contrasena { get; set; } = "";

        [JsonProperty("crfb9_nombre")]
        public string Crfb9_nombre { get; set; } = "";
        [JsonProperty("crfb9_puesto")]
        public string Crfb9_puesto { get; set; } = "";
        [JsonProperty("crfb9_roles")]
        public string Crfb9_roles { get; set; } = "";
        [JsonProperty("crfb9_correo")]
        public string Crfb9_correo { get; set; } = "";
        [JsonProperty("crfb9_area")]
        public string Crfb9_area { get; set; } = "";
        [JsonProperty("crfb9_departamento")]
        public string Crfb9_departamento { get; set; } = "";


    }
}
