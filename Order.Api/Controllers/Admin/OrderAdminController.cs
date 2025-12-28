using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Contracts.Reponsitories;

namespace Order.Api.Controllers.Admin
{
    [Route("api/admin/orders")]
    [ApiController]
    public class OrderAdminController : ControllerBase
    {
        private readonly IDonHangRepository _repo;

        public OrderAdminController(IDonHangRepository repo)
        {
            _repo = repo;
        }

        [HttpPut("{id:int}/status")]
        public IActionResult UpdateStatus(int id, [FromQuery] byte status)
        {
            var ok = _repo.UpdateTrangThai(id, status);
            return Ok(new { success = ok });
        }
    }
}
