using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Infraestructure.Models;

namespace TechShop.Infraestructure.Services
{
    public class DatosPersonalesService : IDatosPersonalesService
    {
        private readonly TechAcademyContext _ctx;
        public DatosPersonalesService(TechAcademyContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<DatosPersonalesDto>> GetAllAsync(bool includeInactive = false)
        {
            var q = _ctx.DatosPersonales.AsQueryable();
            if (!includeInactive)
                q = q.Where(x => x.IsActive);

            return await q
                .Select(e => new DatosPersonalesDto
                {
                    Id = e.Id,
                    Nombres = e.Nombres,
                    Apellidos = e.Apellidos,
                    Email = e.Email,
                    FechaNacimiento = e.FechaNacimiento,
                    IsActive = e.IsActive
                })
                .ToListAsync();
        }

        public async Task<DatosPersonalesDto?> GetByIdAsync(int id)
        {
            var e = await _ctx.DatosPersonales.FindAsync(id);
            if (e == null) return null;
            return new DatosPersonalesDto
            {
                Id = e.Id,
                Nombres = e.Nombres,
                Apellidos = e.Apellidos,
                Email = e.Email,
                FechaNacimiento = e.FechaNacimiento,
                IsActive = e.IsActive
            };
        }

        public async Task<int> CreateAsync(DatosPersonalesDto dto)
        {
            var entity = new DatosPersonales
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                FechaNacimiento = dto.FechaNacimiento,
                IsActive = true
            };
            _ctx.DatosPersonales.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(DatosPersonalesDto dto)
        {
            var e = await _ctx.DatosPersonales.FindAsync(dto.Id);
            if (e == null) return false;
            e.Nombres = dto.Nombres;
            e.Apellidos = dto.Apellidos;
            e.Email = dto.Email;
            e.FechaNacimiento = dto.FechaNacimiento;
            // no tocar e.IsActive aquí por que se nos despicha
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var e = await _ctx.DatosPersonales.FindAsync(id);
            if (e == null) return false;
            e.IsActive = false;
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
