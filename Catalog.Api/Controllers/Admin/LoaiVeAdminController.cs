using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.Admin
{
    [Route("api/admin/loaive")]
    [ApiController]
    public class LoaiVeController : ControllerBase
    {
        private readonly ILoaiVeRepository _repo;

        public LoaiVeController(ILoaiVeRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Create(LoaiVe model)
        {
            model.TrangThai = true;
            var id = _repo.Create(model);
            return Ok(new { success = true, id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, LoaiVe model)
        {
            model.LoaiVeID = id;
            return Ok(new { success = _repo.Update(model) });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(new { success = _repo.Delete(id) });
        }
    }
}
