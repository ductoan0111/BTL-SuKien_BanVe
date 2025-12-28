using Catalog.Application.Contracts.Reponsitories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.User
{
    [Route("api/user/danhmuc-sukien")]
    [ApiController]
    public class DanhMucSuKienController : ControllerBase
    {
        private readonly IDanhMucSuKienRepository _repo;

        public DanhMucSuKienController(IDanhMucSuKienRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _repo.GetAll()
                            .Where(x => x.TrangThai == true)
                            .OrderBy(x => x.ThuTuHienThi);

            return Ok(new
            {
                success = true,
                data
            });
        }

    }
}
