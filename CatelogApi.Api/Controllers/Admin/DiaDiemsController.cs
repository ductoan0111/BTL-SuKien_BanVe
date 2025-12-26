using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catelog.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class DiaDiemsController : ControllerBase
    {
        private readonly IDiaDiemReponsitory _repo;
        public record UpdateTrangThaiRequest(byte TrangThai);
        public DiaDiemsController(IDiaDiemReponsitory repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var data = _repo.GetAll();
            var result = new
            {
                success = true,
                message = "Lấy danh sách địa điểm thành công",
                data
            };
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var data = _repo.GetById(id);
            if (data == null)
            {
                var resultNotFound = new
                {
                    success = false,
                    message = $"Không tìm thấy địa điểm id = {id}"
                };
                return NotFound(resultNotFound);
            }

            var result = new
            {
                success = true,
                message = "Lấy địa điểm thành công",
                data
            };
            return Ok(result);
        }

        [HttpPost]
        public ActionResult Create([FromBody] DiaDiem model)
        {
            var id = _repo.Create(model);

            var result = new
            {
                success = true,
                message = "Tạo địa điểm thành công",
                data = new { id }
            };

            return CreatedAtAction(nameof(GetById), new { id }, result);
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, [FromBody] DiaDiem model)
        {
            model.DiaDiemId = id;

            var ok = _repo.Update(model);
            if (!ok)
            {
                var resultNotFound = new
                {
                    success = false,
                    message = $"Không tìm thấy địa điểm để cập nhật, id = {id}"
                };
                return NotFound(resultNotFound);
            }

            var result = new
            {
                success = true,
                message = "Cập nhật địa điểm thành công",
                data = new { id }
            };
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var ok = _repo.Delete(id);
            if (!ok)
            {
                var resultNotFound = new
                {
                    success = false,
                    message = $"Không tìm thấy địa điểm để xóa, id = {id}"
                };
                return NotFound(resultNotFound);
            }

            var result = new
            {
                success = true,
                message = "Xóa địa điểm thành công",
                data = new { id }
            };
            return Ok(result);
        }


        [HttpPut("{id:int}/trang-thai")]
        public ActionResult UpdateTrangThai(int id, [FromBody] UpdateTrangThaiRequest req)
        {
            var ok = _repo.UpdateTrangThai(id, req.TrangThai);
            if (!ok)
                return NotFound(new { success = false, message = $"Không đổi được trạng thái, id = {id}" });

            return Ok(new
            {
                success = true,
                message = "Đổi trạng thái địa điểm thành công",
                data = new { id, req.TrangThai }
            });
        }
    }
}
