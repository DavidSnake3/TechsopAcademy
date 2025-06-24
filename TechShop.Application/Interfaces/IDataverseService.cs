using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Application.DTOs;

namespace TechShop.Application.Interfaces
{
    public interface IDataverseService
    {
        Task<IEnumerable<EmpleadoDto>> GetEmpleadosAsync();
        Task<EmpleadoDto?> GetEmpleadoByCodigoAsync(string codigo);
    }
}
