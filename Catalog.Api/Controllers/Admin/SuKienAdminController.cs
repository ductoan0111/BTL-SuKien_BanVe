using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class SuKienAdminController : ControllerBase
    {
        private readonly ISuKienRepository _repo;

        public SuKienAdminController(ISuKienRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Create(SuKien model)
        {
            model.TrangThai = 1;
            var id = _repo.Create(model);
            return Ok(new { success = true, id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, SuKien model)
        {
            model.SuKienID = id;
            return Ok(new { success = _repo.Update(model) });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(new { success = _repo.Delete(id) });
        }
    }
}
