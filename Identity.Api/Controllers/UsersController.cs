using Identity.Application.Contracts.Repositories;
using Identity.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class UsersController : ControllerBase
    {
        private readonly INguoiDungRepository _userRepo;

        public UsersController(INguoiDungRepository userRepo)
        {
            _userRepo = userRepo;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userRepo.GetAll();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _userRepo.GetById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateUserRequest request)
        {
            var user = _userRepo.GetById(id);
            if (user == null) return NotFound();

            user.HoTen = request.FullName;
            user.VaiTroID = request.RoleId;
            user.TrangThai = request.TrangThai;
            // ❌ KHÔNG gán user.SoDienThoai nữa vì DB không có cột này

            var ok = _userRepo.Update(user);
            if (!ok) return StatusCode(500, "Không thể cập nhật người dùng.");

            return NoContent();
        }

   
        [HttpDelete("{id:int}")]
        public IActionResult SoftDelete(int id)
        {
            var ok = _userRepo.SoftDelete(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
