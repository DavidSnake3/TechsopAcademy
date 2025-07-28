using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;

namespace TechShop.Infraestructure.Services
{
    public class CursoService : ICursoService
    {
        private readonly TechAcademyContext _ctx;
        public CursoService(TechAcademyContext ctx) => _ctx = ctx;

        public async Task<CursoDetallelDto> GetCursoDetailAsync(int id)
        {
            var cap = await _ctx.Capacitaciones
                .Where(c => c.Id == id)
                .Select(c => new CursoDetallelDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Codigo = c.Codigo,   
                    DescripcionCorta = c.DescripcionCorta,
                    DescripcionLarga = c.DescripcionLarga,
                    DuracionHoras = c.DuracionHoras,
                    Dificultad = c.Dificultad,
                    Activo = c.Activo,
                    FechaCreacion = c.FechaCreacion,
                    Foto = c.Foto,
                    Departamento = c.Departamento,
                    Puestos = c.Puestos,
                    Zonas = c.Zonas,
                    Usuario = c.Usuario
                })
                .FirstOrDefaultAsync();


            if (cap == null) throw new KeyNotFoundException("Curso no encontrado");
            return cap;
        }
    }
}
