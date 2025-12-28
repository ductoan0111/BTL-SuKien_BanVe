using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Contracts;
using Notification.Domain.Entities;

namespace Notification.Api.Controllers.Admin
{
    [Route("api/admin/notifications")]
    [ApiController]
    public class NotificationAdminController : ControllerBase
    {
        private readonly IThongBaoRepository _repo;

        public NotificationAdminController(IThongBaoRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public IActionResult Create([FromBody] ThongBao model)
        {
            var id = _repo.Create(model);
            return Ok(new
            {
                success = true,
                thongBaoId = id
            });
        }
        [HttpGet("user/{nguoiDungId:int}")]
        public IActionResult GetByNguoiDung(int nguoiDungId)
        {
            var list = _repo.GetByNguoiDung(nguoiDungId);
            return Ok(list);
        }
        [HttpPut("{id:int}/sent")]
        public IActionResult MarkAsSent(int id)
        {
            var ok = _repo.MarkAsSent(id);
            return Ok(new { success = ok });
        }
    }
}
