using EventManagement.Organizer.Models.DTOs;
using EventManagement.Organizer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
namespace EventManagement.Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiVeController : ControllerBase
    {
        private readonly ILoaiVeService _service;
        public LoaiVeController(ILoaiVeService service) => _service = service;

        [HttpGet("api/SuKien/{suKienId:int}/LoaiVe")]
        public IActionResult GetBySuKien(int suKienId)
            => Ok(_service.GetBySuKienId(suKienId));

        [HttpGet("api/LoaiVe/{id:int}")]
        public IActionResult GetById(int id)
        {
            var v = _service.GetById(id);
            return v == null ? NotFound() : Ok(v);
        }

        [HttpPost("api/SuKien/{suKienId:int}/LoaiVe")]
        public IActionResult Create(int suKienId, [FromBody] CreateLoaiVeRequests req)
        {
            try
            {
                var id = _service.Create(suKienId, req);
                return Created("", new { LoaiVeID = id });
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // vi phạm unique (SuKienID, TenLoaiVe)
                return Conflict(new { message = "Tên loại vé đã tồn tại trong sự kiện." });
            }
        }

        [HttpPut("api/LoaiVe/{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateLoaiVeRequest req)
        {
            try
            {
                var ok = _service.Update(id, req);
                return ok ? NoContent() : NotFound();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Conflict(new { message = "Tên loại vé đã tồn tại trong sự kiện." });
            }
        }

        [HttpDelete("api/LoaiVe/{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
