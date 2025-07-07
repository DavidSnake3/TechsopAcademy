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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CrearCursoConfigModel model)
        {
            if (model.Equals == null)
            {
                return View(model);
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
            return View("CreateMaterial", model);
        }

        [HttpPost]
        public IActionResult CreateMaterial()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateTest()
        {
            return View();
        }
    }
}
