using System.Collections.Generic;
using System.Threading.Tasks;
using TechShop.Application.DTOs;

namespace TechShop.Application.Interfaces
{
    public interface IAvisoService
    {
        Task<List<AvisoDto>> ObtenerAvisosActivosAsync();
        Task<AvisoDto?> ObtenerAvisoPorIdAsync(int id);
        Task<AvisoDto> CrearAvisoAsync(AvisoDto avisoDto, int empleadoId, string empleadoNombre);
        Task<bool> EliminarAvisoAsync(int id);
        Task<bool> ActualizarAvisoAsync(AvisoDto avisoDto);
        Task<List<AvisoDto>> ObtenerTodosAvisosAsync();
    }
}