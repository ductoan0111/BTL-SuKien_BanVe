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

        public SuKienController(ISuKienService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SuKien>> GetAll()
        {
            var data = _service.GetAll();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public ActionResult<SuKien> GetById(int id)
        {
            var sk = _service.GetById(id);
            if (sk == null) return NotFound();
            return Ok(sk);
        }

        [HttpPost]
        public ActionResult Create([FromBody] SuKien model)
        {
            var ok = _service.Create(model, out string message);
            if (ok) return Ok(new { success = true, message });

            return BadRequest(new { success = false, message });
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] SuKien model)
        {
            if (id != model.SuKienID)
                return BadRequest(new { success = false, message = "ID không khớp." });

            var ok = _service.Update(model, out string message);
            if (ok) return Ok(new { success = true, message });

            return BadRequest(new { success = false, message });
        }
    }
}
