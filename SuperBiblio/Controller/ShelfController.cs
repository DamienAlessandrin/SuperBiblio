using Microsoft.AspNetCore.Mvc;
using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;

namespace SuperBiblio.Controller
{
    [Route("api/[controller]")] // api/shelf
    [ApiController]
    public class ShelfController : ControllerBase
    {
        private readonly IShelfRepository repository;

        public ShelfController(IShelfRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet] // api/shelf
        public async Task<IEnumerable<ShelfModel>> Get()
        {
            return await repository.Get();
        }

        [HttpGet("{id}")] // api/shelf/{id}
        public async Task<ActionResult<ShelfModel>> Get(int id)
        {
            var author = await repository.Get(id);
            if (author == null)
                return NotFound();
            return author;
        }

        [HttpPost] // api/shelf
        public async Task<ActionResult<ShelfModel>> Create(ShelfModel model)
        {
            var author = await repository.Create(model);
            if (author == null)
                return StatusCode(500);
            return model;
        }

    }
}
