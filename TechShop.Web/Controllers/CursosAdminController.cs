using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Infraestructure.Models;
using TechShop.Web.Models;
using TechShop.Web.Models.ViewModels;
using OpcionesRespuesta = TechShop.Infraestructure.Models.OpcionesRespuesta;

namespace TechShop.Web.Controllers
{
    public class CursosAdminController : Controller
    {

        private readonly TechAcademyContext _ctx;
        private readonly ICursoService _cursoService;
        private readonly IDataverseService _dataverse;

        public CursosAdminController(TechAcademyContext ctx, ICursoService cursoService, IDataverseService dataverse)
        {
            _ctx = ctx;
            _cursoService = cursoService;
            _dataverse = dataverse;
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(CrearCursoModel curso)
        {
            var config = curso.config;
            var material = curso.material;
            var examen = curso.examen;

            if (!ModelState.IsValid || config == null || material == null || examen == null)
            {
                return View(curso);
            }

            // Obtener el código del usuario autenticado
            var codigo = User.FindFirst("Codigo")?.Value;
            if (string.IsNullOrEmpty(codigo)) return Forbid();

            // Verificar si ya existe una capacitación con el mismo nombre o código
            var existeCapacitacion = await _ctx.Capacitaciones
                .FirstOrDefaultAsync(c => c.Nombre.ToUpper() == config.Nombre.ToUpper()
                                       || c.Codigo.ToUpper() == config.Codigo.ToUpper());

            if (existeCapacitacion != null)
            {
                ModelState.AddModelError("config.Nombre", "Ya existe una capacitación con ese nombre");
                return View(curso);
            };

            byte[]? fotoBytes = null;
            if (config.FotoFile != null && config.FotoFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await config.FotoFile.CopyToAsync(ms);
                    fotoBytes = ms.ToArray();
                }
            };

            // Crear curso
            var nuevaCapacitacion = new Capacitaciones
            {
                Nombre = config.Nombre,
                Codigo = config.Codigo,
                DescripcionCorta = config.DescripcionCorta,
                DescripcionLarga = config.DescripcionLarga,
                DuracionHoras = config.DuracionHoras,
                Dificultad = config.Dificultad,
                Departamento = JsonConvert.SerializeObject(config.Departamentos),
                Puestos = JsonConvert.SerializeObject(config.Puestos),
                Zonas = JsonConvert.SerializeObject(config.Zonas),
                Activo = true,
                FechaCreacion = DateTime.Now,
                Usuario = codigo,
                Foto = fotoBytes
            };

            // Guardar curso para obtener su id
            _ctx.Capacitaciones.Add(nuevaCapacitacion);
            await _ctx.SaveChangesAsync();

            // Obtener el ID generado
            int cursoId = nuevaCapacitacion.Id;

            // Guardar materiales
            foreach (var seccion in material.Secciones)
            {
                if (seccion != null && seccion.Componentes.Count > 0)
                {
                    //guarda la seccinon
                    var nuevoMaterial = new SeccionCurso
                    {
                        CursoId =cursoId,
                        Titulo = seccion.TituloSeccion,
                        Posicion = 0
                    };
                    _ctx.SeccionCurso.Add(nuevoMaterial);
                    await _ctx.SaveChangesAsync();

                    var seccionId = nuevoMaterial.Id;

                    //guardar los componentes de la seccion
                    foreach (var componente in seccion.Componentes)
                    {
                        if (componente != null)
                        {
                            var nuevoComponente = new MaterialComponente
                            {
                                SeccionId = seccionId,
                                Tipo = componente.Tipo,
                                Contenido = componente.Contenido,
                                Posicion = 0
                            };
                            _ctx.MaterialComponente.Add(nuevoComponente);
                            await _ctx.SaveChangesAsync();
                        }
                    }
                }
            }

            //Guardar el examen
            foreach (var pregunta in examen.Preguntas)
            {
                if (pregunta != null && pregunta.Opciones.Count > 0)
                {
                    var nuevaPregunta = new Preguntas
                    {
                        CapacitacionId = cursoId,
                        TextoPregunta = pregunta.TextoPregunta,
                        TipoPregunta = pregunta.TipoPregunta,
                    };
                    _ctx.Preguntas.Add(nuevaPregunta);
                    await _ctx.SaveChangesAsync();
                    var preguntaId = nuevaPregunta.Id;
                    foreach (var respuesta in pregunta.Opciones)
                    {
                        if (respuesta != null)
                        {
                            var nuevaRespuesta = new OpcionesRespuesta
                            {
                                PreguntaId = preguntaId,
                                TextoOpcion = respuesta.TextoOpcion,
                                EsCorrecta = respuesta.EsCorrecta
                            };
                            _ctx.OpcionesRespuesta.Add(nuevaRespuesta);
                            await _ctx.SaveChangesAsync();
                        }
                    }
                }
            }



            return RedirectToAction("index");
        }
    }
}
