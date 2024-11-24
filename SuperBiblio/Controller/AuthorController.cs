using Microsoft.AspNetCore.Mvc;
using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;

namespace SuperBiblio.Controller
{
    [Route("api/[controller]")] // api/author
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository repository;

        public AuthorController(IAuthorRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet] // api/author
        public async Task<IEnumerable<AuthorModel>> Get()
        {
            return await repository.Get();
        }

        [HttpGet("{id}")] // api/author/{id}
        public async Task<ActionResult<AuthorModel>> Get(int id)
        {
            var author = await repository.Get(id);
            if (author == null)
                return NotFound();
            return author;
        }

        [HttpPost] // api/author
        public async Task<ActionResult<AuthorModel>> Create(AuthorModel model)
        {
            var author = await repository.Create(model);
            if (author == null)
                return StatusCode(500);
            return model;
        }
    }
}