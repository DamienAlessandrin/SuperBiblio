using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories
{
    public interface IMemberRepository
    {
        Task<IEnumerable<MemberModel>> Get();

        Task<MemberModel?> Get(int id);

        Task<MemberModel?> Create(MemberModel model);
    }
}
