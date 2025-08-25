using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Infraestructure.Models;
using TechShop.Web.Models;
using TechShop.Web.Models.ViewModels;

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
            var vm = new Models.MaterialCursoViewModel
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

        [HttpGet]
        public async Task<IActionResult> Examen(int id)
        {
            var capacitacion = await _ctx.Capacitaciones
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (capacitacion == null)
            {
                return NotFound(); 
            }

            var preguntas = await _ctx.Preguntas
                .Where(p => p.CapacitacionId == id)
                .Include(p => p.OpcionesRespuesta)
                .OrderBy(p => p.Id)
                .ToListAsync();

            var vm = new ExamenVM
            {
                CapacitacionId = capacitacion.Id,
                Nombre = capacitacion.Nombre,            
                Codigo = capacitacion.Codigo,             
                DescripcionCorta = capacitacion.DescripcionCorta,      
                Preguntas = preguntas.Select(p => new PreguntaVM
                {
                    PreguntaId = p.Id,
                    TextoPregunta = p.TextoPregunta,
                    TipoPregunta = p.TipoPregunta,
                    Opciones = p.OpcionesRespuesta.Select(o => new OpcionVM
                    {
                        OpcionId = o.Id,
                        TextoOpcion = o.TextoOpcion
                    }).ToList()
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Examen(ExamenVM model)
        {
            var codigo = User.FindFirst("Codigo")?.Value;
            if (string.IsNullOrEmpty(codigo)) return Forbid();

            var emp = await _dataverse.GetEmpleadoByCodigoAsync(codigo);
            if (emp == null || !int.TryParse(emp.Crfb9_codigo, out var empleadoId)) return Forbid();





            var ahora = DateTime.Now;

            var preguntasBD = await _ctx.Preguntas
                .Include(p => p.OpcionesRespuesta)
                .Where(p => p.CapacitacionId == model.CapacitacionId)
                .ToListAsync();

            var detalles = new List<ResultadoDetalleVM>();
            int total = 0, correctas = 0;

            foreach (var preguntaVM in model.Preguntas)
            {
                var preguntaBD = preguntasBD.FirstOrDefault(p => p.Id == preguntaVM.PreguntaId);
                if (preguntaBD == null) continue;

                total++;
                bool esCorrecta = false;
                string respuestaUsuario = "";
                string respuestaCorrecta = "";

                if (preguntaVM.TipoPregunta == "Unica")
                {
                    var opcionCorrecta = preguntaBD.OpcionesRespuesta.FirstOrDefault(o => o.EsCorrecta);
                    respuestaCorrecta = opcionCorrecta?.TextoOpcion;

                    var opcionUsuario = preguntaBD.OpcionesRespuesta
                        .FirstOrDefault(o => o.Id == preguntaVM.OpcionSeleccionada);
                    respuestaUsuario = opcionUsuario?.TextoOpcion;

                    if (opcionCorrecta != null && opcionUsuario?.Id == opcionCorrecta.Id)
                        esCorrecta = true;
                }
                else if (preguntaVM.TipoPregunta == "Multiple")
                {
                    var opcionesCorrectas = preguntaBD.OpcionesRespuesta.Where(p => p.EsCorrecta).Select(p => p.Id).ToList();
                    var opcionesUsuario = preguntaVM.OpcionesSeleccionadas ?? new List<int>();

                    // Se considera correcta solo si el conjunto es idéntico
                    if (opcionesCorrectas.All(op => opcionesUsuario.Contains(op)) &&
                        opcionesUsuario.All(op => opcionesCorrectas.Contains(op)))
                    {
                        esCorrecta = true;
                    }

                    respuestaCorrecta = string.Join(", ",
                        preguntaBD.OpcionesRespuesta.Where(o => opcionesCorrectas.Contains(o.Id))
                        .Select(o => o.TextoOpcion));

                    respuestaUsuario = string.Join(", ",
                        preguntaBD.OpcionesRespuesta.Where(o => opcionesUsuario.Contains(o.Id))
                        .Select(o => o.TextoOpcion));
                }

                if (esCorrecta) correctas++;

                detalles.Add(new ResultadoDetalleVM
                {
                    TextoPregunta = preguntaBD.TextoPregunta,
                    RespuestaUsuario = respuestaUsuario,
                    RespuestaCorrecta = respuestaCorrecta,
                    EsCorrecta = esCorrecta
                });
            }

            decimal nota = total > 0 ? Math.Round(correctas * 100m / total, 2) : 0m;
            bool aprobado = nota >= 70m;

            if (aprobado)
            {
                foreach (var p in model.Preguntas)
                {
                    if (p.TipoPregunta == "Unica" && p.OpcionSeleccionada.HasValue)
                    {
                        _ctx.RespuestasEmpleado.Add(new RespuestasEmpleado
                        {
                            EmpleadoId = empleadoId,
                            PreguntaId = p.PreguntaId,
                            OpcionRespuestaId = p.OpcionSeleccionada,
                            FechaRespuesta = ahora
                        });
                    }
                    else if (p.TipoPregunta == "Multiple" && p.OpcionesSeleccionadas?.Any() == true)
                    {
                        foreach (var opcionId in p.OpcionesSeleccionadas)
                        {
                            _ctx.RespuestasEmpleado.Add(new RespuestasEmpleado
                            {
                                EmpleadoId = empleadoId,
                                PreguntaId = p.PreguntaId,
                                OpcionRespuestaId = opcionId,
                                FechaRespuesta = ahora
                            });
                        }
                    }
                }

                _ctx.ResultadosCapacitacion.Add(new ResultadosCapacitacion
                {
                    EmpleadoId = empleadoId,
                    CapacitacionId = model.CapacitacionId,
                    Nota = nota,
                    Aprobado = true,
                    FechaEvaluacion = ahora
                });

                var hist = await _ctx.HistorialCapacitacionEmpleado
                    .FirstOrDefaultAsync(h =>
                        h.EmpleadoId == empleadoId &&
                        h.CapacitacionId == model.CapacitacionId &&
                        h.Estado == "En Proceso");

                if (hist != null)
                {
                    hist.Estado = "Aprobado";
                    hist.FechaCompletado = ahora;
                }

                await _ctx.SaveChangesAsync();
            }

            return View("Resultados", new ResultadosVM
            {
                CapacitacionId = model.CapacitacionId,
                Nombre = model.Nombre,
                Codigo = model.Codigo,
                TotalPreguntas = total,
                Correctas = correctas,
                Nota = nota,
                Aprobado = aprobado,
                Detalles = detalles
            });
        }

        [HttpGet]
        public async Task<IActionResult> Resultados(int id)
        {
            var codigo = User.FindFirst("Codigo")?.Value;
            if (string.IsNullOrEmpty(codigo)) return Forbid();
            var emp = await _dataverse.GetEmpleadoByCodigoAsync(codigo);
            if (emp == null || !int.TryParse(emp.Crfb9_codigo, out var empleadoId))
                return Forbid();

            var cap = await _ctx.Capacitaciones
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (cap == null)
            {
                return NotFound();
            }


            var res = await _ctx.ResultadosCapacitacion
                .Where(r => r.EmpleadoId == empleadoId && r.CapacitacionId == id)
                .FirstOrDefaultAsync();

            var preguntasMC = await _ctx.Preguntas
                .Where(p => p.CapacitacionId == id && p.TipoPregunta == "MultipleChoice")
                .ToListAsync();

            var detalles = new List<ResultadoDetalleVM>();
            foreach (var p in preguntasMC)
            {
                var resp = await _ctx.RespuestasEmpleado
                    .Where(r => r.EmpleadoId == empleadoId && r.PreguntaId == p.Id)
                    .OrderByDescending(r => r.FechaRespuesta)
                    .FirstOrDefaultAsync();
                if (resp == null) continue;

                var correcta = await _ctx.OpcionesRespuesta
                    .Where(o => o.PreguntaId == p.Id && o.EsCorrecta)
                    .Select(o => o.TextoOpcion)
                    .FirstOrDefaultAsync();

                string usuarioText = resp.OpcionRespuestaId.HasValue
                    ? await _ctx.OpcionesRespuesta
                        .Where(o => o.Id == resp.OpcionRespuestaId.Value)
                        .Select(o => o.TextoOpcion)
                        .FirstOrDefaultAsync()
                    : resp.RespuestaTexto;

                bool esOK = await _ctx.OpcionesRespuesta
                    .Where(o => o.Id == resp.OpcionRespuestaId)
                    .Select(o => o.EsCorrecta)
                    .FirstOrDefaultAsync();

                detalles.Add(new ResultadoDetalleVM
                {
                    TextoPregunta = p.TextoPregunta,
                    RespuestaUsuario = usuarioText,
                    RespuestaCorrecta = correcta,
                    EsCorrecta = esOK
                });
            }

            var vm = new ResultadosVM
            {
                Nombre = cap.Nombre,
                Codigo = cap.Codigo,
                CapacitacionId = id,
                TotalPreguntas = detalles.Count,
                Correctas = detalles.Count(d => d.EsCorrecta),
                Nota = res?.Nota ?? 0m,
                Aprobado = res?.Aprobado ?? false,
                Detalles = detalles
            };

            return View(vm);
        }


    }
}
