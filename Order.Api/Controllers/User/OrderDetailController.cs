using Microsoft.AspNetCore.Mvc;
using Order.Application.Contracts.Reponsitories;

namespace Order.Api.Controllers.User
{
    [ApiController]
    [Route("api/user/orders/{donHangId:int}/items")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IChiTietDonHangRepository _repo;

        public OrderDetailController(IChiTietDonHangRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetItems(int donHangId)
        {
            var data = _repo.GetByDonHang(donHangId);
            return Ok(new { success = true, data });
        }
    }
}
