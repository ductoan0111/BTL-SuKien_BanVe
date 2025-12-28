using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Contracts.Reponsitories;
using Order.Domain.Entities;

namespace Order.Api.Controllers.User
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IDonHangRepository _donHangRepo;

        public OrderController(IDonHangRepository repo)
        {
            _donHangRepo = repo;
        }

        [HttpGet("{nguoiMuaId:int}")]
        public IActionResult GetMyOrders(int nguoiMuaId)
        {
            var data = _donHangRepo.GetByNguoiMua(nguoiMuaId);
            return Ok(new { success = true, data });
        }

        [HttpPost]
        public IActionResult Create(DonHang model)
        {
            model.TrangThai = 0;
            var id = _donHangRepo.Create(model);
            return Ok(new { success = true, orderId = id });
        }
    }
}
