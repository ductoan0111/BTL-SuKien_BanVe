using Events_Management.Models;
using Events_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuKienController : ControllerBase
    {
        private readonly ISuKienService _service;
        public SuKienController(ISuKienService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var sk = _service.GetById(id);
            return sk == null ? NotFound() : Ok(sk);
        }

        [HttpPost]
        public IActionResult Create([FromBody] SuKien sk)
        {
            var newId = _service.Create(sk);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { SuKienID = newId });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] SuKien sk)
            => _service.Update(id, sk) ? NoContent() : NotFound();

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
