using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;

namespace TechShop.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class EmpleadosController : Controller
    {
        private readonly IDataverseService _svc;
        public EmpleadosController(IDataverseService svc) => _svc = svc;

        //public async Task<IActionResult> Index()
        //{
        //    IEnumerable<EmpleadoDto> lista = await _svc.GetEmpleadosAsync();
        //    return View(lista);
        //}

        // READ
        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1, int pageSize = 20)
        {
            var all = (await _svc.GetEmpleadosAsync());
                //.Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();
                all = all.Where(p =>
                    p.Crfb9_nombre.ToLower().Contains(searchString.ToLower()) ||
                    p.Crfb9_codigo.ToLower().Contains(searchString.ToLower()) ||
                    (p.Crfb9_correo?.ToLower().Contains(searchString.ToLower()) ?? false));
            }

            var count = all.Count();
            var items = all
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["CurrentFilter"] = searchString;
            ViewData["PageNumber"] = pageNumber;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling(count / (double)pageSize);

            return View(items);
        }
    }
}
