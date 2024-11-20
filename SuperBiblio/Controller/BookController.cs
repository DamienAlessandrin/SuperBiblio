using Microsoft.AspNetCore.Mvc;
using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;

namespace SuperBiblio.Controller
{
    [Route("api/[controller]")] // api/book
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository repository;

        public BookController(IBookRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet] // api/book
        public async Task<IEnumerable<BookModel>> Get()
        {
            return await repository.Get();
        }

        [HttpGet("{id}")] // api/book/{id}
        public async Task<ActionResult<BookModel>> Get(int id)
        {
            var model = await repository.Get(id);
            if (model == null)
                return NotFound();
            return model;
        }

        [HttpPost] // api/book
        public async Task<ActionResult<BookModel>> Create(BookModel model)
        {
            var book = await repository.Create(model);
            if (book == null)
                return StatusCode(500);
            return model;
        }
    }
}