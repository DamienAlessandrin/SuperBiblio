using Microsoft.EntityFrameworkCore;
using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories.Sql
{
    public class SqlAuthorRepository : IAuthorRepository
    {
        private readonly DataContext context;

        public SqlAuthorRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<AuthorModel?> Create(AuthorModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AuthorModel>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthorModel?> Get(int id)
        {
            return await context.Author.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
