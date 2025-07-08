using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Interfaces;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
    public class CursoController : Controller
    {
        private readonly ICursoService _cursoService;
        public CursoController(ICursoService cursoService) => _cursoService = cursoService;

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _cursoService.GetCursoDetailAsync(id);
            var vm = new CursoDetailViewModel
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Codigo = dto.Codigo,
                DescripcionCorta = dto.DescripcionCorta,
                DescripcionLarga = dto.DescripcionLarga,
                DuracionHoras = dto.DuracionHoras,
                Dificultad = dto.Dificultad,
                FechaCreacion = dto.FechaCreacion,
                Materiales = dto.Materiales,
                Preguntas = dto.Preguntas
            };
            return View(vm);
        }
    }
}
