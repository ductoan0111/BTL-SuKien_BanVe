using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.Admin
{
    [Route("api/admin/danhmuc-sukien")]
    [ApiController]
    public class DanhMucSuKienAdminController : ControllerBase
    {
        private readonly IDanhMucSuKienRepository _repo;

        public DanhMucSuKienAdminController(IDanhMucSuKienRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new { success = true, data = _repo.GetAll() });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null)
                return Ok(new { success = false, message = "Không tìm thấy danh mục" });

            return Ok(new { success = true, data = item });
        }

        [HttpPost]
        public IActionResult Create(DanhMucSuKien model)
        {
            model.TrangThai = true;
            var id = _repo.Create(model);
            return Ok(new { success = true, id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, DanhMucSuKien model)
        {
            model.DanhMucID = id;
            return Ok(new { success = _repo.Update(model) });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(new { success = _repo.Delete(id) });
        }
    }
}
