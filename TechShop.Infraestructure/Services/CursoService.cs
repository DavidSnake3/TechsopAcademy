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

        public async Task<CursoDetailDto> GetCursoDetailAsync(int id)
        {
            var cap = await _ctx.Capacitaciones
                .Where(c => c.Id == id)
                .Select(c => new CursoDetailDto
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
                    Materiales = c.MaterialCurso.Select(m => new MaterialDto
                    {
                        Id = m.Id,
                        NombreArchivo = m.NombreArchivo,
                        RutaArchivo = m.RutaArchivo,
                        TipoArchivo = m.TipoArchivo,
                        FechaSubida = m.FechaSubida,
                         VideoUrl = m.VideoUrl 
                    }).ToList(),
                    Preguntas = c.Preguntas.Select(p => new PreguntaDto
                    {
                        Id = p.Id,
                        TextoPregunta = p.TextoPregunta,
                        TipoPregunta = p.TipoPregunta,
                        Opciones = p.OpcionesRespuesta.Select(o => new OpcionRespuestaDto
                        {
                            Id = o.Id,
                            TextoOpcion = o.TextoOpcion,
                            EsCorrecta = o.EsCorrecta
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (cap == null) throw new KeyNotFoundException("Curso no encontrado");
            return cap;
        }
    }
}
