using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;

namespace TechShop.Infraestructure.Services
{
    public class DataverseService : IDataverseService
    {
        private readonly HttpClient _http;
        public DataverseService(HttpClient http) => _http = http;

        public async Task<IEnumerable<EmpleadoDto>> GetEmpleadosAsync()
        {
            var response = await _http.PostAsync(
                "",
                new StringContent("{}", Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var wrapper = JsonConvert.DeserializeObject<EmpleadosWrapper>(json)!;
            return wrapper.Empleados;
        }
    }

    public class EmpleadosWrapper
    {
        [JsonProperty("empleados")]
        public List<EmpleadoDto> Empleados { get; set; } = new();
    }
}
