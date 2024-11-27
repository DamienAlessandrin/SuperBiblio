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
            var shelf = await repository.Get(id);
            if (shelf == null)
                return NotFound();
            return shelf;
        }

        [HttpPost] // api/shelf
        public async Task<ActionResult<ShelfModel>> Create(ShelfModel model)
        {
            var shelf = await repository.Create(model);
            if (shelf == null)
                return StatusCode(500);
            return model;
        }

    }
}
