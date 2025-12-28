using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.Admin
{
    [Route("api/admin/diadiem")]
    [ApiController]
    public class DiaDiemAdminController : ControllerBase
    {
        private readonly IDiaDiemReponsitory _repo;

        public DiaDiemAdminController(IDiaDiemReponsitory repo)
        {
            _repo = repo;
        }

        // GET: api/admin/diadiem
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _repo.GetAll();
            return Ok(new { success = true, data });
        }

        // GET: api/admin/diadiem/5
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null)
                return Ok(new { success = false, message = "Không tìm thấy địa điểm" });

            return Ok(new { success = true, data = item });
        }

        // POST: api/admin/diadiem
        [HttpPost]
        public IActionResult Create([FromBody] DiaDiem model)
        {
            // mặc định active
            model.TrangThai = true;

            var newId = _repo.Create(model);
            return Ok(new { success = true, message = "Tạo địa điểm thành công", id = newId });
        }

        // PUT: api/admin/diadiem/5
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] DiaDiem model)
        {
            model.DiaDiemID = id;

            var ok = _repo.Update(model);
            if (!ok)
                return Ok(new { success = false, message = "Cập nhật thất bại" });

            return Ok(new { success = true, message = "Cập nhật thành công" });
        }

        // DELETE: api/admin/diadiem/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var ok = _repo.Delete(id);
            if (!ok)
                return Ok(new { success = false, message = "Xóa thất bại" });

            return Ok(new { success = true, message = "Xóa thành công" });
        }
    }
}
