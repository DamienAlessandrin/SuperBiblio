using Microsoft.EntityFrameworkCore;
using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories.Sql
{
    public class SqlBookRepository : IBookRepository
    {
        private readonly DataContext context;

        public SqlBookRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<BookModel?> Create(BookModel model)
        {
            context.Set<BookModel>().Add(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<BookModel>> Get()
        {
            return await context.Book
                .Include(x => x.Author)
                .Include(x => x.Shelf)
                .Include(x => x.Member)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BookModel?> Get(int id)
        {
            return await context.Book
                .Include(x => x.Author)
                .Include(x => x.Shelf)
                .Include(x => x.Member)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BookModel>> GetByTitle(string title)
        {
            return await context.Book
                .Include(x => x.Author)
                .Include(x => x.Shelf)
                .Include(x => x.Member)
                .Where(x => EF.Functions.Like(x.Title, $"%{title}%"))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetForAuthor(int authorId)
        {
            return await context.Book
                .AsNoTracking()
                .Where(x => x.AuthorModelId == authorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetForShelf(int shelfId)
        {
            return await context.Book
                .AsNoTracking()
                .Where(x => x.ShelfModelId == shelfId)
                .ToListAsync();
        }

        public async Task<BookModel?> Update(int id, BookModel model)
        {
            var person = await context.Set<BookModel>().FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
                return null;

            context.Entry(person).CurrentValues.SetValues(model);
            await context.SaveChangesAsync();
            return model;
        }
    }
}