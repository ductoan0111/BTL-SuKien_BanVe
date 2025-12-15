using Events_Management.Models;
using Events_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucSuKienController : ControllerBase
    {
        private readonly IDanhMucSuKienService _service;

        public DanhMucSuKienController(IDanhMucSuKienService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var dm = _service.GetById(id);
            return dm == null ? NotFound() : Ok(dm);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DanhMucSuKien dm)
        {
            var newId = _service.Create(dm);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { DanhMucID = newId });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] DanhMucSuKien dm)
            => _service.Update(id, dm) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
