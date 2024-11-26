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

        [HttpGet] // api/book ou api/book?authorId=...
        public async Task<IEnumerable<BookModel>> Get([FromQuery] int? authorId, int? shelfId)
        {
            if (authorId.HasValue)
            {
                return await repository.GetForAuthor(authorId.Value);
            }

            if (shelfId.HasValue)
            {
                return await repository.GetForShelf(shelfId.Value);
            }

            return await repository.Get();
        }

        [HttpGet("{id}")] // api/book/{id}
        public async Task<ActionResult<BookModel>> Get(int id)
        {
            var book = await repository.Get(id);
            if (book == null)
                return NotFound();
            return book;
        }

        [HttpPost] // api/book
        public async Task<ActionResult<BookModel>> Create(BookModel model)
        {
            var book = await repository.Create(model);
            if (book == null)
                return StatusCode(500);
            return model;
        }

        [HttpPut("{id}")] // api/book/{id}
        public async Task<ActionResult<BookModel>> Update(int id, BookModel model)
        {
            var person = await repository.Update(id, model);
            if (person == null)
                return NotFound();
            return model;
        }
    }
}