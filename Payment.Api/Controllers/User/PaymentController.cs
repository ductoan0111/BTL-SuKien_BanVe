using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Contracts.Repositories;
using Payment.Domain.Entities;

namespace Payment.Api.Controllers.User
{
    [Route("api/user/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IThanhToanRepository _repo;

        public PaymentController(IThanhToanRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ThanhToan model)
        {
            model.TrangThai = 0; // Khởi tạo
            var id = _repo.Create(model);

            return Ok(new
            {
                success = true,
                paymentId = id
            });
        }

        [HttpGet("order/{donHangId:int}")]
        public IActionResult GetByOrder(int donHangId)
        {
            var data = _repo.GetByDonHang(donHangId);
            return Ok(new { success = true, data });
        }
    }
}
