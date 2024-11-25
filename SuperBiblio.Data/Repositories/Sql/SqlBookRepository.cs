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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BookModel?> Get(int id)
        {
            //return await context.Book.FirstOrDefaultAsync(x => x.Id == id); // author serrait vide
            return await context.Book
                .Include(x => x.Author)
                .Include(x => x.Shelf)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
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