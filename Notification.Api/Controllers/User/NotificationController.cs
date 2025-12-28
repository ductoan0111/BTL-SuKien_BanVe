using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Contracts;

namespace Notification.Api.Controllers.User
{
    [Route("api/user/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IThongBaoRepository _repo;

        public NotificationController(IThongBaoRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public IActionResult GetMyNotifications([FromQuery] int nguoiDungId)
        {
            var list = _repo.GetByNguoiDung(nguoiDungId);
            return Ok(list);
        }
    }
}
