using Microsoft.AspNetCore.Mvc;
using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;

namespace SuperBiblio.Controller
{
    [Route("api/[controller]")] // api/member
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository repository;

        public MemberController(IMemberRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet] // api/member
        public async Task<IEnumerable<MemberModel>> Get()
        {
            return await repository.Get();
        }

        [HttpGet("{id}")] // api/member/{id}
        public async Task<ActionResult<MemberModel>> Get(int id)
        {
            var member = await repository.Get(id);
            if (member == null)
                return NotFound();
            return member;
        }

        [HttpPost] // api/member
        public async Task<ActionResult<MemberModel>> Create(MemberModel model)
        {
            var member = await repository.Create(model);
            if (member == null)
                return StatusCode(500);
            return model;
        }
    }
}
