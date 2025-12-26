using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catelog.Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoaiVesController : ControllerBase
    {
        private readonly ILoaiVeRepository _repo;

        public LoaiVesController(ILoaiVeRepository repo)
        {
            _repo = repo;
        }

        // GET: /api/LoaiVes/by-su-kien/5
        [HttpGet("by-su-kien/{suKienId:int}")]
        public ActionResult<List<LoaiVe>> GetBySuKien(int suKienId)
        {
            var data = _repo.GetBySuKien(suKienId);
            return Ok(data);
        }

        // GET: /api/LoaiVes/10
        [HttpGet("{id:int}")]
        public ActionResult<LoaiVe> GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
