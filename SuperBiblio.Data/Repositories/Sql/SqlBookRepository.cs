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
            return await context.Book.ToListAsync();
        }

        public async Task<BookModel?> Get(int id)
        {
            return await context.Book.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}