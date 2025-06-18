using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Interfaces;

namespace TechShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IDataverseService _svc;
        public EmpleadosController(IDataverseService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lista = await _svc.GetEmpleadosAsync();
            return Ok(lista);
        }
    }
}
