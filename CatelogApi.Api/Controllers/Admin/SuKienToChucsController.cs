using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catelog.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuKienToChucsController : ControllerBase
    {
        private readonly ISuKienToChucRepository _repo;

        public SuKienToChucsController(ISuKienToChucRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetBySuKien(int suKienId)
        {
            var data = _repo.GetBySuKien(suKienId);
            return Ok(new { success = true, data });
        }

        [HttpGet("{nguoiDungId:int}")]
        public IActionResult Get(int suKienId, int nguoiDungId)
        {
            var item = _repo.Get(suKienId, nguoiDungId);
            if (item == null)
                return Ok(new { success = false, message = "Không tìm thấy ban tổ chức." });

            return Ok(new { success = true, data = item });
        }

        public record AddToChucRequest(int NguoiDungId, string VaiTroTrongSuKien);

        [HttpPost]
        public IActionResult Create(int suKienId, [FromBody] AddToChucRequest req)
        {
            var ok = _repo.Create(new SuKienToChuc
            {
                SuKienId = suKienId,
                NguoiDungId = req.NguoiDungId,
                VaiTroTrongSuKien = req.VaiTroTrongSuKien
            });

            if (!ok)
                return Ok(new { success = false, message = "Thêm không thành công (có thể đã tồn tại)." });

            return Ok(new { success = true, message = "Thêm ban tổ chức thành công." });
        }

        public record UpdateRoleRequest(string VaiTroTrongSuKien);
        [HttpPut("{nguoiDungId:int}")]
        public IActionResult UpdateRole(int suKienId, int nguoiDungId, [FromBody] UpdateRoleRequest req)
        {
            var ok = _repo.Update(new SuKienToChuc
            {
                SuKienId = suKienId,
                NguoiDungId = nguoiDungId,
                VaiTroTrongSuKien = req.VaiTroTrongSuKien
            });

            if (!ok)
                return Ok(new { success = false, message = "Cập nhật không thành công (không tìm thấy)." });

            return Ok(new { success = true, message = "Cập nhật vai trò thành công." });
        }
        [HttpDelete("{nguoiDungId:int}")]
        public IActionResult Delete(int suKienId, int nguoiDungId)
        {
            var ok = _repo.Delete(suKienId, nguoiDungId);

            if (!ok)
                return Ok(new { success = false, message = "Xóa không thành công (không tìm thấy)." });

            return Ok(new { success = true, message = "Xóa ban tổ chức thành công." });
        }

    }
}
