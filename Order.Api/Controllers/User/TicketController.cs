using Microsoft.AspNetCore.Mvc;
using Order.Application.Contracts.Reponsitories;

namespace Order.Api.Controllers.User
{
    [Route("api/user/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IVeRepository _repo;

        public TicketController(IVeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{nguoiDungId:int}")]
        public IActionResult GetMyTickets(int nguoiDungId)
        {
            var data = _repo.GetByNguoiSoHuu(nguoiDungId);
            return Ok(new { success = true, data });
        }
    }
}
