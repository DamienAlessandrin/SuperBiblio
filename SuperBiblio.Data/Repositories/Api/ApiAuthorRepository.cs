using SuperBiblio.Data.Models;
using System.Net.Http.Json;

namespace SuperBiblio.Data.Repositories.Api
{
    public class ApiAuthorRepository : IAuthorRepository
    {
        private readonly string url;
        private readonly HttpClient client; // Permet d'envoyer et de recevoir les requêtes HTTP

        public ApiAuthorRepository(string url)
        {
            this.url = url;
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }

        public async Task<AuthorModel?> Create(AuthorModel model)
        {
            var response = await client.PostAsJsonAsync($"{url}author", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AuthorModel>();
            return null;
        }

        public async Task<IEnumerable<AuthorModel>> Get()
        {
            var response = await client.GetAsync($"{url}author");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<AuthorModel>>();
            return null;
        }

        public async Task<AuthorModel?> Get(int id)
        {
            var response = await client.GetAsync($"{url}author/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AuthorModel>();
            return null;
        }
    }
}