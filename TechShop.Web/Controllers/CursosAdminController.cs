using Microsoft.AspNetCore.Mvc;

namespace TechShop.Web.Controllers
{
    public class CursosAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
