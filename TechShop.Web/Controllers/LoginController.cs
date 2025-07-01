using Libreria.Infraestructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechShop.Application.Interfaces;

public class LoginController : Controller
{
    private readonly IDataverseService _dataverse;
    public LoginController(IDataverseService dataverse) => _dataverse = dataverse;

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index() => View();

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginTest login)
    {
        var empleado = await _dataverse.GetEmpleadoByCodigoAsync(login.User);
        if (empleado == null || empleado.Crfb9_contrasena != login.Password)
        {
            ViewBag.MessageUser = empleado == null ? "* Usuario no existe" : null;
            ViewBag.MessagePassword = empleado != null && empleado.Crfb9_contrasena != login.Password
                                     ? "* Contraseña incorrecta"
                                     : null;
            return View(login);
        }

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, empleado.Crfb9_nombre),
            new Claim("Codigo", empleado.Crfb9_codigo),
            new Claim(ClaimTypes.Role, empleado.Crfb9_rol ?? "Empleado"),
            new Claim("Puesto", empleado.Crfb9_puesto ?? "Empleado"),
        };
        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync("MyCookieAuth", principal);

        TempData["Mensaje"] = @"
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true
            });
            Toast.fire({
                icon: 'success',
                title: 'Bienvenido " + empleado.Crfb9_nombre + @"'
            });
        ";

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Index", "Login");
    }

    public IActionResult AccessDenied() => View();
}
