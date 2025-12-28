using Catalog.Application.Contracts.Reponsitories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.User
{
    [Route("api/user/sukien")]
    [ApiController]
    public class SuKienController : ControllerBase
    {
        private readonly ISuKienRepository _repo;
        public SuKienController(ISuKienRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new
            {
                success = true,
                data = _repo.GetAll(onlyActive: true)
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null || item.TrangThai == 0)
                return Ok(new { success = false, message = "Không tìm thấy sự kiện" });

            return Ok(new { success = true, data = item });
        }
    }
}
