using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
    public class CursoController : Controller
    {
        private readonly TechAcademyContext _ctx;
        private readonly ICursoService _cursoService;

        public CursoController(TechAcademyContext ctx, ICursoService cursoService)
        {
            _ctx = ctx;
            _cursoService = cursoService;
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var dto = await _cursoService.GetCursoDetailAsync(id);
            var vm = new CrearCursoConfigModel
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Codigo = dto.Codigo,
                DescripcionCorta = dto.DescripcionCorta,
                DescripcionLarga = dto.DescripcionLarga,
                DuracionHoras = dto.DuracionHoras,
                Dificultad = dto.Dificultad,
                FechaCreacion = dto.FechaCreacion,
                Foto = dto.Foto,
                Departamentos = !string.IsNullOrWhiteSpace(dto.Departamento)
        ? JsonConvert.DeserializeObject<List<string>>(dto.Departamento)
        : new List<string>(),

                Zonas = !string.IsNullOrWhiteSpace(dto.Zonas)
        ? JsonConvert.DeserializeObject<List<string>>(dto.Zonas)
        : new List<string>(),

                Puestos = !string.IsNullOrWhiteSpace(dto.Puestos)
        ? JsonConvert.DeserializeObject<List<string>>(dto.Puestos)
        : new List<string>(),

                Usuario = dto.Usuario,
                Materiales = dto.Materiales,
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Imagen(int id)
        {
            var foto = await _ctx.Capacitaciones
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => c.Foto)
                .FirstOrDefaultAsync();

            if (foto != null && foto.Length > 0)
                return File(foto, "image/jpeg");

            return File("~/images/no-image.png", "image/png");
        }

    }
}
