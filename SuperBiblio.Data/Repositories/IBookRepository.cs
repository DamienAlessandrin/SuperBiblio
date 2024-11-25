using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookModel>> Get();

        Task<BookModel?> Get(int id);

        Task<BookModel?> Create(BookModel model);*
          
        Task<IEnumerable<BookModel>> GetForAuthor(int authorId);

        Task<BookModel?> Update(int id, BookModel model);
    }
}