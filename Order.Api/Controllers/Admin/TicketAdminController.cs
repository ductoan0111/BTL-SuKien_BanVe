using Microsoft.AspNetCore.Mvc;
using Order.Application.Contracts.Reponsitories;

namespace Order.Api.Controllers.Admin
{
    [Route("api/admin/tickets")]
    [ApiController]
    public class TicketAdminController : Controller
    {
        private readonly IVeRepository _repo;

        public TicketAdminController(IVeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("scan")]
        public IActionResult ScanQr([FromQuery] string qrToken)
        {
            var ve = _repo.GetByQrToken(qrToken);
            if (ve == null)
                return Ok(new { success = false, message = "Vé không hợp lệ" });

            if (ve.TrangThai == 1)
                return Ok(new { success = false, message = "Vé đã sử dụng" });

            _repo.UpdateTrangThai(ve.VeID, 1);

            return Ok(new { success = true, message = "Check-in thành công", ve });
        }
    }
}
