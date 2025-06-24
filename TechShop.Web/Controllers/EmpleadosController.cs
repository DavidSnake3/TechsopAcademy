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

        public async Task<IActionResult> Index()
        {
            IEnumerable<EmpleadoDto> lista = await _svc.GetEmpleadosAsync();
            return View(lista);
        }
    }
}
