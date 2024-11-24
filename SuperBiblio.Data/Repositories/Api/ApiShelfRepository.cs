using SuperBiblio.Data.Models;
using System.Net.Http.Json;

namespace SuperBiblio.Data.Repositories.Api
{
    public class ApiShelfRepository : IShelfRepository
    {
        private readonly string url;
        private readonly HttpClient client;

        public ApiShelfRepository(string url)
        {
            this.url = url;
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }

        public async Task<ShelfModel?> Create(ShelfModel model)
        {
            var response = await client.PostAsJsonAsync($"{url}shelf", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ShelfModel>();
            return null;
        }

        public async Task<IEnumerable<ShelfModel>> Get()
        {
            var response = await client.GetAsync($"{url}shelf");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<ShelfModel>>();
            return null;
        }

        public async Task<ShelfModel?> Get(int id)
        {
            var response = await client.GetAsync($"{url}shelf/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ShelfModel>();
            return null;
        }
    }
}
