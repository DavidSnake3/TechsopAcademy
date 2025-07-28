using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TechShop.Application.DTOs;
using TechShop.Infraestructure.Data;
using TechShop.Infraestructure.Models;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TechAcademyContext _ctx;
        private readonly AvisosService _avisosSvc;
        public HomeController(ILogger<HomeController> logger, TechAcademyContext ctx, AvisosService avisosSvc)
        {
            _logger = logger;
            _ctx = ctx;
            _avisosSvc = avisosSvc;
        }

        public async Task<IActionResult> Index()
        {
            var empleadoId = int.Parse(User.Claims.First(c => c.Type == "Codigo").Value);

            // resultados aprobados
            var completados = await (
                from r in _ctx.ResultadosCapacitacion
                join c in _ctx.Capacitaciones on r.CapacitacionId equals c.Id
                where r.EmpleadoId == empleadoId && r.Aprobado
                select new CursoDetallelDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    DescripcionCorta = c.DescripcionCorta,
                    FechaCreacion = c.FechaCreacion,
                    DuracionHoras = c.DuracionHoras,
                    Codigo = c.Codigo,
                    Dificultad = c.Dificultad,
                    Foto = c.Foto
                }
            ).ToListAsync();


            // en proceso
            var enProceso = await (
                from h in _ctx.HistorialCapacitacionEmpleado
                join c in _ctx.Capacitaciones on h.CapacitacionId equals c.Id
                where h.EmpleadoId == empleadoId && h.FechaCompletado == null
                select new CursoDetallelDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    DescripcionCorta = c.DescripcionCorta,
                    FechaCreacion = c.FechaCreacion,
                    DuracionHoras = c.DuracionHoras,
                    Codigo = c.Codigo,
                    Dificultad = c.Dificultad,
                    Foto = c.Foto
                }
            ).ToListAsync();

            // disponible 
            var disponibles = await (
                from c in _ctx.Capacitaciones
                where c.Activo
                   && !_ctx.HistorialCapacitacionEmpleado
                       .Any(h => h.EmpleadoId == empleadoId && h.CapacitacionId == c.Id)
                select new CursoDetallelDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    DescripcionCorta = c.DescripcionCorta,
                    FechaCreacion = c.FechaCreacion,
                    DuracionHoras = c.DuracionHoras,
                    Codigo = c.Codigo,
                    Dificultad = c.Dificultad,
                    Foto = c.Foto
                }
            ).ToListAsync();

            var vm = new DashboardViewModel
            {
                Completados = completados,
                EnProceso = enProceso,
                Disponibles = disponibles
            };

            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            vm.Avisos = _avisosSvc.ObtenerAvisos();
            return View(vm);
        }

        [HttpPost]
        public IActionResult AgregarAviso(string texto)
        {
            var codigo = User.Claims.FirstOrDefault(c => c.Type == "Codigo")?.Value;
            if (codigo != "2693" && codigo != "2692")
                return Forbid();

            if (!string.IsNullOrWhiteSpace(texto))
                _avisosSvc.AgregarAviso(texto);

            TempData["Mensaje"] = "Swal.fire('Aviso agregado','Tu aviso se mostrará durante 7 días','success');";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarAviso(long fechaTicks)
        {
            _avisosSvc.EliminarAviso(new DateTime(fechaTicks));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditarAviso(long fechaTicks, string texto)
        {
            _avisosSvc.EditarAviso(new DateTime(fechaTicks), texto);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Cursos(
            string estado = "disponibles",
            string searchString = null)
        {
            var empleadoId = int.Parse(User.Claims.First(c => c.Type == "Codigo").Value);
            var model = new DashboardViewModel();

            IQueryable<Capacitaciones> query = estado.ToLower() switch
            {
                "enproceso" => from h in _ctx.HistorialCapacitacionEmpleado
                               join c in _ctx.Capacitaciones on h.CapacitacionId equals c.Id
                               where h.EmpleadoId == empleadoId && h.FechaCompletado == null
                               select c,

                "finalizados" => from r in _ctx.ResultadosCapacitacion
                                 join c in _ctx.Capacitaciones on r.CapacitacionId equals c.Id
                                 where r.EmpleadoId == empleadoId && r.Aprobado
                                 select c,

                _ => _ctx.Capacitaciones.Where(c =>
                         c.Activo &&
                         !_ctx.HistorialCapacitacionEmpleado
                              .Any(h => h.EmpleadoId == empleadoId && h.CapacitacionId == c.Id))
            };

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(c =>
                    c.Nombre.Contains(searchString) ||
                    c.Codigo.Contains(searchString));
            }

            var cursosFiltrados = await query
                .Select(c => new CursoDetallelDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    DescripcionCorta = c.DescripcionCorta,
                    FechaCreacion = c.FechaCreacion,
                    DuracionHoras = c.DuracionHoras,
                    Codigo = c.Codigo,
                    Dificultad = c.Dificultad,
                    Foto = c.Foto
                })
                .ToListAsync();

            switch (estado.ToLower())
            {
                case "enproceso":
                    model.EnProceso = cursosFiltrados;
                    break;
                case "finalizados":
                    model.Completados = cursosFiltrados;
                    break;
                default:
                    model.Disponibles = cursosFiltrados;
                    break;
            }

            ViewBag.Estado = estado;
            ViewBag.SearchString = searchString;
            return View("~/Views/Curso/Cursos.cshtml", model);
        }
    }
    }
