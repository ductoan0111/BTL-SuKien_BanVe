using EventManagement.Organizer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;
        public ReportController(IReportService service) => _service = service;

        [HttpGet("SuKien/{id:int}/Sales")]
        public IActionResult Sales(int id) => Ok(_service.Sales(id));

        [HttpGet("SuKien/{id:int}/Attendance")]
        public IActionResult Attendance(int id) => Ok(_service.Attendance(id));
    }
}
