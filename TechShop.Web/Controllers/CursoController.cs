using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Infraestructure.Models;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
    public class CursoController : Controller
    {
        private readonly TechAcademyContext _ctx;
        private readonly ICursoService _cursoService;
        private readonly IDataverseService _dataverse;

        public CursoController(TechAcademyContext ctx, ICursoService cursoService, IDataverseService dataverse)
        {
            _ctx = ctx;
            _cursoService = cursoService;
            _dataverse = dataverse;
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var estadoProceso = await EstaCapacitacionEnProcesoAsync(id);
            var dto = await _cursoService.GetCursoDetailAsync(id);
            var vm = new CrearCursoConfigModel
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Codigo = dto.Codigo,
                DescripcionCorta = dto.DescripcionCorta,
                DescripcionLarga = dto.DescripcionLarga,
                DuracionHoras = dto.DuracionHoras,
                Dificultad = dto.Dificultad,
                FechaCreacion = dto.FechaCreacion,
                Foto = dto.Foto,
                Departamentos = !string.IsNullOrWhiteSpace(dto.Departamento)
                ? dto.Departamento
                     .Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => s.Trim())
                     .ToList()
                : new List<string>(),

                Puestos = !string.IsNullOrWhiteSpace(dto.Puestos)
                ? dto.Puestos
                     .Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => s.Trim())
                     .ToList()
                : new List<string>(),

                Zonas = !string.IsNullOrWhiteSpace(dto.Zonas)
                ? dto.Zonas
                     .Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => s.Trim())
                     .ToList()
                : new List<string>(),

                Usuario = dto.Usuario,
                Materiales = dto.Materiales,
                EstadoProceso = estadoProceso
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Imagen(int id)
        {
            var foto = await _ctx.Capacitaciones
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => c.Foto)
                .FirstOrDefaultAsync();

            if (foto != null && foto.Length > 0)
                return File(foto, "image/jpeg");

            return File("~/images/no-image.png", "image/png");
        }

        [HttpGet]
        public async Task<IActionResult> Material(int id)
        {
            var secciones = await _ctx.SeccionCurso
                .AsNoTracking()
                .Where(s => s.CursoId == id)
                .OrderBy(s => s.Posicion)
                .Select(s => new SeccionMaterialViewModel
                {
                    Id = s.Id,
                    CapacitacionId = s.CursoId,
                    Titulo = s.Titulo.Trim(),
                    Posicion = s.Posicion,
                    Componentes = s.MaterialComponente
                        .OrderBy(c => c.Posicion)
                        .Select(c => new ComponenteMaterialViewModel
                        {
                            Id = c.Id,
                            SeccionId = c.SeccionId ?? 0,
                            Tipo = c.Tipo.Trim(),
                            Contenido = c.Contenido,
                            Posicion = c.Posicion ?? 0
                        })
                        .ToList()
                })
                .ToListAsync();

            if (!secciones.Any())
                return NotFound("No se encontraron secciones para este curso.");

            var cap = await _ctx.Capacitaciones
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    c.Codigo,
                    c.DescripcionCorta
                })
                .FirstOrDefaultAsync();

            if (cap == null)
                return NotFound("Capacitación no encontrada.");

            var estado = await EstaCapacitacionEnProcesoAsync(id);
            var vm = new MaterialCursoViewModel
            {
                CapacitacionId = cap.Id,
                Nombre = cap.Nombre,
                Codigo = cap.Codigo,
                DescripcionCorta = cap.DescripcionCorta,
                Secciones = secciones,
                EstadoProceso = estado
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Comenzar(int capacitacionId)
        {
            var codigoClaim = User.FindFirst("Codigo")?.Value;
            if (string.IsNullOrEmpty(codigoClaim))
                return Forbid();

            var empleado = await _dataverse.GetEmpleadoByCodigoAsync(codigoClaim);
            if (empleado == null)
                return Forbid();

            if (!int.TryParse(empleado.Crfb9_codigo, out var empleadoIdInt))
                return BadRequest("Código de empleado inválido.");

            var historial = new HistorialCapacitacionEmpleado
            {
                EmpleadoId = empleadoIdInt,
                CapacitacionId = capacitacionId,
                FechaAsignacion = DateTime.Now,
                Estado = "En Proceso",
                FechaCompletado = null
            };

            _ctx.HistorialCapacitacionEmpleado.Add(historial);
            await _ctx.SaveChangesAsync();

            return RedirectToAction("Material", new { id = capacitacionId });
        }

        private async Task<bool> EstaCapacitacionEnProcesoAsync(int capacitacionId)
        {
            var codigoClaim = User.FindFirst("Codigo")?.Value;
            if (string.IsNullOrEmpty(codigoClaim))
                return false;

            var empleado = await _dataverse.GetEmpleadoByCodigoAsync(codigoClaim);
            if (empleado == null || !int.TryParse(empleado.Crfb9_codigo, out var empleadoIdInt))
                return false;

            return await _ctx.HistorialCapacitacionEmpleado
                .AnyAsync(h =>
                    h.EmpleadoId == empleadoIdInt &&
                    h.CapacitacionId == capacitacionId &&
                    h.Estado == "En Proceso");
        }
    }
}
