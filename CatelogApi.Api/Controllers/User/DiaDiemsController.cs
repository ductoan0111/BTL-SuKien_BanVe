using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catelog.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaDiemsController : ControllerBase
    {
        private readonly IDiaDiemReponsitory _repo;

        public DiaDiemsController(IDiaDiemReponsitory repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<List<DiaDiem>> GetAll()
        {
            var data = _repo.GetAll();
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public ActionResult<DiaDiem> GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
