using Libreria.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Libreria.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginTest login)
        {
            if (login.User == null || login.Password == null)
            {
                ViewBag.MessageUser = "* Por favor, debe indicar el usuario";
                ViewBag.MessagePassword = "* Por favor, debe indicar la contraseña";
                return View("index");
            }

            if ((!login.User.Equals("admin") || login.User.IsNullOrEmpty()) && (!login.Password.Equals("123456") || login.Password.IsNullOrEmpty()))
            {
                ViewBag.MessageUser = "* Usuario incorrecto.";
                ViewBag.MessagePassword = "* Contraseña incorrecta.";
                return View("index");
            }
           else if (!login.User.Equals("admin") || login.User.IsNullOrEmpty())
            {
                ViewBag.MessageUser = "* Usuario incorrecto.";
                return View("index");
            }
            else if (!login.Password.Equals("123456") || login.Password.IsNullOrEmpty())
            {
                ViewBag.MessagePassword = "* Contraseña incorrecta.";
                return View("index");
            }
            else
            {
                TempData["Mensaje"] = "const Toast = Swal.mixin({toast: true, position: \"top-end\", showConfirmButton: false, timer: 3000, timerProgressBar: true, didOpen: (toast) => {toast.onmouseenter = Swal.stopTimer; toast.onmouseleave = Swal.resumeTimer;}});\n" +
            "Toast.fire({icon: \"success\", title: \"Bienvenido " + login.User + "\"});";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
