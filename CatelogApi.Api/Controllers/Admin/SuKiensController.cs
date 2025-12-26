using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catelog.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class SuKiensController : ControllerBase
    {
        private readonly ISuKienRepository _repo;

        public SuKiensController(ISuKienRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var data = _repo.GetAll();
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách sự kiện thành công",
                data
            });
        }

        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var data = _repo.GetById(id);
            if (data == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Không tìm thấy sự kiện id = {id}"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Lấy sự kiện thành công",
                data
            });
        }

        [HttpPost]
        public ActionResult Create([FromBody] SuKien model)
        {
            var id = _repo.Create(model);

            return Ok(new
            {
                success = true,
                message = "Tạo sự kiện thành công",
                data = new { id }
            });
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, [FromBody] SuKien model)
        {
            model.SuKienId = id;
            var ok = _repo.Update(model);

            if (!ok)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Không tìm thấy sự kiện để cập nhật, id = {id}"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Cập nhật sự kiện thành công",
                data = new { id }
            });
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var ok = _repo.Delete(id);

            if (!ok)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Không tìm thấy sự kiện để xóa, id = {id}"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Xóa sự kiện thành công",
                data = new { id }
            });
        }
    }
}
