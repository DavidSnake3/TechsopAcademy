using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Web.Models;



namespace TechShop.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Roles = "ADMIN")]
    public class CursosAdminController : Controller
    {

        public IActionResult Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateMaterial()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateTest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CrearCursoConfigModel config)
        {
            if (config.Equals == null)
            {
                return View();
            }
            var datos = new
            {
                config.Nombre,
                config.Codigo,
                config.DescripcionCorta,
                config.DescripcionLarga,
                config.DuracionHoras,
                config.Dificultad,
                Departamentos = config.Departamentos != null ? string.Join(", ", config.Departamentos) : "Ninguno",
                Roles = config.Roles != null ? string.Join(", ", config.Roles) : "Ninguno",
                Zonas = config.Zonas != null ? string.Join(", ", config.Zonas) : "Ninguno"
            };

            var mensaje = $@"Swal.fire({{
                title: 'Curso creado',
                html: 'Nombre: {datos.Nombre}<br/>Código: {datos.Codigo}<br/>Descripción: {datos.DescripcionCorta}<br/>Duración: {datos.DuracionHoras} horas<br/>Dificultad: {datos.Dificultad}<br/>Departamentos: {datos.Departamentos}<br/>Roles: {datos.Roles}<br/>Zonas: {datos.Zonas}',
                icon: 'success'
            }})";

            TempData["Mensaje"] = mensaje;
            return View();
        }

        [HttpPost]
        public IActionResult CreateMaterial(CrearMaterialViewModel material)
        {
            if (material.Secciones.LongCount() == 0)
            {
                return View("CreateMaterial");
            }
            return View("CreateTest");
        }

        [HttpPost]
        public IActionResult CreateTest(int a)
        {
            return View();
        }
    }
}
