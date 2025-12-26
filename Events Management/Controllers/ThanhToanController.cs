using Events_Management.Models.DTOs;
using Events_Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly IThanhToanService _service;
        public ThanhToanController(IThanhToanService service) => _service = service;

        [HttpPost("mock-success/{donHangId:int}")]
        public IActionResult MockSuccess(int donHangId, [FromBody] MockThanhToanRequest? req)
        {
            var result = _service.MockSuccess(donHangId, req);
            return Ok(result);
        }
    }
}
