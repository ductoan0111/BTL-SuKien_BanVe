using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catelog.Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuKiensController : ControllerBase
    {
        private readonly ISuKienRepository _suKienRepo;

        public SuKiensController(ISuKienRepository suKienRepo)
        {
            _suKienRepo = suKienRepo;
        }

        [HttpGet]
        [HttpGet]
        public ActionResult<List<SuKien>> GetAll([FromQuery] byte? trangThai)
        {
            var data = _suKienRepo.GetAll();

            if (trangThai.HasValue)
            {
                var flag = trangThai.Value == 1;
                data = data.Where(x => x.TrangThai == flag).ToList();
            }

            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public ActionResult<SuKien> GetById(int id)
        {
            var item = _suKienRepo.GetById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("by-dia-diem/{diaDiemId:int}")]
        public ActionResult<List<SuKien>> GetByDiaDiem(int diaDiemId)
        {
            var data = _suKienRepo.GetByDiaDiem(diaDiemId);
            return Ok(data);
        }
    }
}
