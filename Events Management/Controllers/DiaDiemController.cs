using Events_Management.Models;
using Events_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaDiemController : ControllerBase
    {
        private readonly IDiaDiemService _service;

        public DiaDiemController(IDiaDiemService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var dd = _service.GetById(id);
            return dd == null ? NotFound() : Ok(dd);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DiaDiem dd)
        {
            var newId = _service.Create(dd);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { DiaDiemID = newId });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] DiaDiem dd)
            => _service.Update(id, dd) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
