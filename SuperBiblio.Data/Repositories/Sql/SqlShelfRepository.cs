using Microsoft.EntityFrameworkCore;
using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories.Sql
{
    public class SqlShelfRepository : IShelfRepository
    {
        private readonly DataContext context;

        public SqlShelfRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<ShelfModel?> Create(ShelfModel model)
        {
            context.Set<ShelfModel>().Add(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<ShelfModel>> Get()
        {
            return await context.Shelf.ToListAsync();
        }

        public async Task<ShelfModel?> Get(int id)
        {
            return await context.Shelf.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}