using Microsoft.AspNetCore.Mvc;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {

        [Route("{code:int}")]
        public IActionResult Index(int code)
        {
            string message = code switch
            {
                400 => "Solicitud incorrecta",
                401 => "No estás autenticado",
                403 => "Acceso denegado",
                404 => "Lo sentimos, página no encontrada",
                500 => "Lo sentimos, error interno del servidor",
                _ => "Ocurrió un error"
            };

            var vm = new ErrorViewModel
            {
                ErrorCode = code.ToString(),
                Message = message,
            };

            return View("ErrorView", vm);
        }

    }
}
