using Microsoft.EntityFrameworkCore;
using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories.Sql
{
    public class SqlMemberRepository : IMemberRepository
    {
        private readonly DataContext context;

        public SqlMemberRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<MemberModel?> Create(MemberModel model)
        {
            context.Set<MemberModel>().Add(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<MemberModel>> Get()
        {
            return await context.Member.ToListAsync();
        }

        public async Task<MemberModel?> Get(int id)
        {
            return await context.Member.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
