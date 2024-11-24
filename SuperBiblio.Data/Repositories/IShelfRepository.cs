using SuperBiblio.Data.Models;

namespace SuperBiblio.Data.Repositories
{
    public interface IShelfRepository
    {
        Task<IEnumerable<ShelfModel>> Get();

        Task<ShelfModel?> Get(int id);

        Task<ShelfModel?> Create(ShelfModel model);
    }
}