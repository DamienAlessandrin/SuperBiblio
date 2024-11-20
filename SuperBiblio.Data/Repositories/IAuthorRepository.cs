using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorModel>> Get();

        Task<AuthorModel?> Get(int id);

        Task<AuthorModel?> Create(AuthorModel model);
    }
}