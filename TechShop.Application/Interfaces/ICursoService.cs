using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Application.DTOs;

namespace TechShop.Application.Interfaces
{
    public interface ICursoService
    {
        Task<CursoDetailDto> GetCursoDetailAsync(int id);
    }
}
