using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;
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
        private readonly IAvisoService _avisoService;

        public HomeController(ILogger<HomeController> logger, TechAcademyContext ctx, IAvisoService avisoService)
        {
            _logger = logger;
            _ctx = ctx;
            _avisoService = avisoService;
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

            var avisos = await _avisoService.ObtenerAvisosActivosAsync();

            var vm = new DashboardViewModel
            {
                Completados = completados,
                EnProceso = enProceso,
                Disponibles = disponibles,
                Avisos = avisos
            };

            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarAviso(AvisoDto avisoDto)
        {
            try
            {
                var codigo = User.Claims.FirstOrDefault(c => c.Type == "Codigo")?.Value;
                if (codigo != "2693" && codigo != "2692")
                    return Forbid();

                if (!ModelState.IsValid)
                {
                    TempData["Mensaje"] = "Swal.fire('Error','Por favor complete todos los campos correctamente','error');";
                    return RedirectToAction("Index");
                }

                var empleadoId = int.Parse(codigo!);
                var empleadoNombre = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;

                await _avisoService.CrearAvisoAsync(avisoDto, empleadoId, empleadoNombre);

                TempData["Mensaje"] = "Swal.fire('Éxito','Aviso agregado correctamente','success');";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar aviso");
                TempData["Mensaje"] = "Swal.fire('Error','No se pudo agregar el aviso','error');";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarAviso(int id)
        {
            try
            {
                var codigo = User.Claims.FirstOrDefault(c => c.Type == "Codigo")?.Value;
                if (codigo != "2693" && codigo != "2692")
                    return Forbid();

                var resultado = await _avisoService.EliminarAvisoAsync(id);

                if (resultado)
                    TempData["Mensaje"] = "Swal.fire('Éxito','Aviso eliminado correctamente','success');";
                else
                    TempData["Mensaje"] = "Swal.fire('Error','No se pudo encontrar el aviso','error');";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar aviso");
                TempData["Mensaje"] = "Swal.fire('Error','No se pudo eliminar el aviso','error');";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerAviso(int id)
        {
            var aviso = await _avisoService.ObtenerAvisoPorIdAsync(id);
            if (aviso == null)
                return NotFound();

            return Json(aviso);
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