using Identity.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IVaiTroRepository _roleRepo;

        public RolesController(IVaiTroRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleRepo.GetAll();
            return Ok(roles);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var role = _roleRepo.GetById(id);
            if (role == null) return NotFound();

            return Ok(role);
        }
    }
}
