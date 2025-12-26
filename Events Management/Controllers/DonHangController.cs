using Events_Management.Models.DTOs;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangService _service;
        public DonHangController(IDonHangService service) => _service = service;

        [HttpPost]
        public IActionResult Create([FromBody] CreateDonHangRequest req)
        {
            var id = _service.Create(req);
            return CreatedAtAction(nameof(GetDetail), new { id }, new { DonHangID = id });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetDetail(int id)
        {
            var result = _service.GetDetail(id);
            return Ok(new { donHang = result.donHang, items = result.items });
        }

        [HttpGet("by-user/{nguoiMuaId:int}")]
        public IActionResult GetByUser(int nguoiMuaId)
            => Ok(_service.GetByNguoiMua(nguoiMuaId));
        [HttpGet("{id:int}/Ve")]
        public IActionResult GetVeByDonHang(int id, [FromServices] IVeRepository veRepo)
        {
            return Ok(veRepo.GetByDonHangId(id));
        }
    }
}
