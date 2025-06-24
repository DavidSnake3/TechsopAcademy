using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TechShop.Infraestructure.Services
{
    public class DataverseService : IDataverseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DataverseService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<EmpleadoDto?> GetEmpleadoByCodigoAsync(string codigo)
        {
            var json = JsonConvert.SerializeObject(new { codigo });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = _configuration["Dataverse:Endpoint"];
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmpleadoResponse>(responseString);
            return result?.Empleados?.FirstOrDefault();
        }

        public async Task<IEnumerable<EmpleadoDto>> GetEmpleadosAsync()
        {
            var url = _configuration["Dataverse:GetAllEmpleadosEndpoint"];

            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<EmpleadoDto>();
            }

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<EmpleadoResponse>(responseString);
            return result?.Empleados ?? new List<EmpleadoDto>();
        }
    }

    public class EmpleadoResponse
    {
        [JsonProperty("empleados")]
        public List<EmpleadoDto> Empleados { get; set; } = new();
    }
}
