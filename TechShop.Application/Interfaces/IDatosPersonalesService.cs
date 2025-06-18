using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Application.DTOs;

namespace TechShop.Application.Interfaces
{
    public interface IDatosPersonalesService
    {
        //Task<IEnumerable<DatosPersonalesDto>> GetAllAsync();
        Task<IEnumerable<DatosPersonalesDto>> GetAllAsync(bool includeInactive = false);
        Task<DatosPersonalesDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(DatosPersonalesDto dto);
        Task<bool> UpdateAsync(DatosPersonalesDto dto);
        Task<bool> SoftDeleteAsync(int id);
    }
}
