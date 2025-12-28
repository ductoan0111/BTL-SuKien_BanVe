using Catalog.Application.Contracts.Reponsitories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.User
{
    [Route("api/loaive")]
    [ApiController]
    public class LoaiVeController : ControllerBase
    {
        private readonly ILoaiVeRepository _repo;
        public LoaiVeController(ILoaiVeRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("sukien/{suKienId:int}")]
        public IActionResult GetBySuKien(int suKienId)
        {
            return Ok(new
            {
                success = true,
                data = _repo.GetBySuKien(suKienId, onlyActive: true)
            });
        }
    }
}
