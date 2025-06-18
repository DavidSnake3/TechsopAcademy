using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Application.DTOs;
using TechShop.Application.Interfaces;
using TechShop.Infraestructure.Data;
using TechShop.Web.Models;

namespace TechShop.Web.Controllers
{
    public class DatosPersonalesController : Controller
    {
        private readonly IDatosPersonalesService _svc;
        public DatosPersonalesController(IDatosPersonalesService svc) => _svc = svc;

        // READ
        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1, int pageSize = 5)
        {
            var all = (await _svc.GetAllAsync())
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();
                all = all.Where(p =>
                    p.Nombres.ToLower().Contains(searchString) ||
                    p.Apellidos.ToLower().Contains(searchString) ||
                    (p.Email?.ToLower().Contains(searchString) ?? false));
            }

            var count = all.Count();
            var items = all
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["CurrentFilter"] = searchString;
            ViewData["PageNumber"] = pageNumber;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling(count / (double)pageSize);

            return View(items);
        }

        // DETALLE
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        // CREATE
        public IActionResult Create() => View();
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatosPersonalesDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // UPDATE
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DatosPersonalesDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            if (!await _svc.UpdateAsync(dto)) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        // DESACTIVAR
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
