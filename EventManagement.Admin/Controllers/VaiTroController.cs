using EventManagement.Admin.Models;
using EventManagement.Admin.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EventManagement.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroService _service;
        public VaiTroController(IVaiTroService service) => _service = service;

        // GET /api/VaiTro?trangThai=true&keyword=adm
        [HttpGet]
        public IActionResult GetAll([FromQuery] bool? trangThai, [FromQuery] string? keyword)
            => Ok(_service.GetAll(trangThai, keyword));

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var v = _service.GetById(id);
            return v == null ? NotFound() : Ok(v);
        }

        [HttpPost]
        public IActionResult Create([FromBody] VaiTro v)
        {
            try
            {
                var id = _service.Create(v);
                return CreatedAtAction(nameof(GetById), new { id }, new { VaiTroID = id });
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Conflict(new { message = "TenVaiTro đã tồn tại (unique)." });
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] VaiTro v)
        {
            try
            {
                var ok = _service.Update(id, v);
                return ok ? NoContent() : NotFound();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Conflict(new { message = "TenVaiTro đã tồn tại (unique)." });
            }
        }

        // soft delete
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
