using Microsoft.AspNetCore.Mvc;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
    public class CursosAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CrearCursoConfigModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return View("CreateMaterial", model);
        }

        public IActionResult CreateMaterial()
        {
            return View();
        }
        public IActionResult CreateTest()
        {
            return View();
        }
    }
}
