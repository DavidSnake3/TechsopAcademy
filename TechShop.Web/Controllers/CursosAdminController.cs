using Microsoft.AspNetCore.Mvc;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
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
        public IActionResult Create(CrearCursoConfigModel model)
        {
            if (model.Equals == null)
            {
                return View();
            }
            var datos = new
            {
                model.Nombre,
                model.Codigo,
                model.DescripcionCorta,
                model.DescripcionLarga,
                model.DuracionHoras,
                model.Dificultad,
                Departamentos = model.Departamentos != null ? string.Join(", ", model.Departamentos) : "Ninguno",
                Roles = model.Roles != null ? string.Join(", ", model.Roles) : "Ninguno",
                Zonas = model.Zonas != null ? string.Join(", ", model.Zonas) : "Ninguno"
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
        public IActionResult CreateMaterial(int a)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
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
