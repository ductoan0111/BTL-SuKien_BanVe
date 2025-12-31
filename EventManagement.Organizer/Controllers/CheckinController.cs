using EventManagement.Organizer.Models.DTOs;
using EventManagement.Organizer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckinController : ControllerBase
    {
        private readonly ICheckinService _service;
        public CheckinController(ICheckinService service) => _service = service;

        [HttpPost]
        public IActionResult Checkin([FromBody] CheckinRequest req)
            => Ok(_service.Checkin(req));
    }
}
