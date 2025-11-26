using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Infraestructure.Models;

namespace TechShop.Infraestructure.Services
{
    public class AvisoService : IAvisoService
    {
        private readonly TechAcademyContext _context;

        public AvisoService(TechAcademyContext context)
        {
            _context = context;
        }

        public async Task<List<AvisoDto>> ObtenerAvisosActivosAsync()
        {
            var ahora = DateTime.UtcNow;
            
            var avisos = await _context.Avisos
                .Where(a => a.FechaExpiracion > ahora)
                .OrderByDescending(a => a.FechaCreacion)
                .Select(a => new AvisoDto
                {
                    Id = a.Id,
                    Texto = a.Texto,
                    FechaCreacion = a.FechaCreacion,
                    FechaExpiracion = a.FechaExpiracion,
                    CreadoPorNombre = a.CreadoPorNombre,
                    EstaActivo = a.EstaActivo
                })
                .ToListAsync();

            return avisos;
        }

        public async Task<AvisoDto?> ObtenerAvisoPorIdAsync(int id)
        {
            var aviso = await _context.Avisos
                .Where(a => a.Id == id)
                .Select(a => new AvisoDto
                {
                    Id = a.Id,
                    Texto = a.Texto,
                    FechaCreacion = a.FechaCreacion,
                    FechaExpiracion = a.FechaExpiracion,
                    CreadoPorNombre = a.CreadoPorNombre,
                    EstaActivo = a.EstaActivo
                })
                .FirstOrDefaultAsync();

            return aviso;
        }

        public async Task<AvisoDto> CrearAvisoAsync(AvisoDto avisoDto, int empleadoId, string empleadoNombre)
        {
            var aviso = new Aviso
            {
                Texto = avisoDto.Texto.Trim(),
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddDays(avisoDto.DuracionDias),
                CreadoPorEmpleadoId = empleadoId,
                CreadoPorNombre = empleadoNombre
            };

            _context.Avisos.Add(aviso);
            await _context.SaveChangesAsync();

            avisoDto.Id = aviso.Id;
            avisoDto.FechaCreacion = aviso.FechaCreacion;
            avisoDto.FechaExpiracion = aviso.FechaExpiracion;
            avisoDto.CreadoPorNombre = aviso.CreadoPorNombre;
            avisoDto.EstaActivo = aviso.EstaActivo;

            return avisoDto;
        }

        public async Task<bool> EliminarAvisoAsync(int id)
        {
            var aviso = await _context.Avisos.FindAsync(id);
            if (aviso == null)
                return false;

            _context.Avisos.Remove(aviso);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarAvisoAsync(AvisoDto avisoDto)
        {
            var aviso = await _context.Avisos.FindAsync(avisoDto.Id);
            if (aviso == null)
                return false;

            aviso.Texto = avisoDto.Texto.Trim();
            aviso.FechaExpiracion = aviso.FechaCreacion.AddDays(avisoDto.DuracionDias);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AvisoDto>> ObtenerTodosAvisosAsync()
        {
            var avisos = await _context.Avisos
                .OrderByDescending(a => a.FechaCreacion)
                .Select(a => new AvisoDto
                {
                    Id = a.Id,
                    Texto = a.Texto,
                    FechaCreacion = a.FechaCreacion,
                    FechaExpiracion = a.FechaExpiracion,
                    CreadoPorNombre = a.CreadoPorNombre,
                    EstaActivo = a.EstaActivo
                })
                .ToListAsync();

            return avisos;
        }
    }
}