using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Contracts.Repositories;

namespace Payment.Api.Controllers.Admin
{
    [Route("api/admin/payments")]
    [ApiController]
    public class PaymentAdminController : ControllerBase
    {
        private readonly IThanhToanRepository _repo;

        public PaymentAdminController(IThanhToanRepository repo)
        {
            _repo = repo;
        }

        [HttpPut("{id:int}/status")]
        public IActionResult UpdateStatus(
            int id,
            [FromQuery] byte status,
            [FromBody] string? rawResponse)
        {
            var ok = _repo.UpdateStatus(id, status, rawResponse);
            return Ok(new { success = ok });
        }
    }
}
