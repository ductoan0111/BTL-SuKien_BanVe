using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catelog.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class LoaiVesController : ControllerBase
    {
        private readonly ILoaiVeRepository _repo;

        public LoaiVesController(ILoaiVeRepository repo)
        {
            _repo = repo;
        }

        // GET: api/admin/LoaiVes
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _repo.GetAll();
            return Ok(new { success = true, data });
        }

        // GET: api/admin/LoaiVes/by-su-kien/5
        [HttpGet("by-su-kien/{suKienId:int}")]
        public IActionResult GetBySuKien(int suKienId)
        {
            var data = _repo.GetBySuKien(suKienId);
            return Ok(new { success = true, data });
        }

        // GET: api/admin/LoaiVes/10
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null)
                return Ok(new { success = false, message = "Không tìm thấy loại vé." });

            return Ok(new { success = true, data = item });
        }

        [HttpPost]
        public IActionResult Create([FromBody] LoaiVe model)
        {
            var id = _repo.Create(model);
            return Ok(new { success = true, message = "Tạo loại vé thành công.", id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] LoaiVe model)
        {
            model.LoaiVeId = id;
            var ok = _repo.Update(model);

            if (!ok)
                return Ok(new { success = false, message = "Cập nhật thất bại (không tìm thấy hoặc đã bị xóa)." });

            return Ok(new { success = true, message = "Cập nhật loại vé thành công." });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var ok = _repo.Delete(id);

            if (!ok)
                return Ok(new { success = false, message = "Xóa thất bại (không tìm thấy hoặc đã bị xóa)." });

            return Ok(new { success = true, message = "Xóa loại vé thành công (đã đổi trạng thái = false)." });
        }
    }
}
