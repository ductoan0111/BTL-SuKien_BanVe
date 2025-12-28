using Catalog.Application.Contracts.Reponsitories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.User
{
    [Route("api/diadiem")]
    [ApiController]
    public class DiaDiemController : ControllerBase
    {
        private readonly IDiaDiemReponsitory _repo;

        public DiaDiemController(IDiaDiemReponsitory repo)
        {
            _repo = repo;
        }

        // GET: api/user/diadiem
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _repo.GetAll(); // nếu muốn chỉ active thì sửa repo lọc TrangThai=1
            return Ok(new { success = true, data });
        }

        // GET: api/user/diadiem/5
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null)
                return Ok(new { success = false, message = "Không tìm thấy địa điểm" });

            return Ok(new { success = true, data = item });
        }
    }
}
