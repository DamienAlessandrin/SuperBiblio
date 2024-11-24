using SuperBiblio.Data.Models;
using SuperBiblio.Data.Repositories;
using System.Net.Http.Json;

namespace EfCore.Data.Repositories
{
    public class ApiBookRepository : IBookRepository
    {
        private readonly string url;
        private readonly HttpClient client;

        public ApiBookRepository(string url)
        {
            this.url = url;
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }

        public async Task<BookModel?> Create(BookModel model)
        {
            //Console.WriteLine($"{model.Id} {model.Title} {model.AuthorModelId} {model.Author.Id} {model.Author.FirstName}");
            var response = await client.PostAsJsonAsync($"{url}book", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<BookModel>();
            return null;
        }

        public async Task<IEnumerable<BookModel>> Get()
        {
            var response = await client.GetAsync($"{url}book");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<BookModel>>();
            return null;
        }

        public async Task<BookModel?> Get(int id)
        {
            var reponse = await client.GetAsync($"{url}book/{id}");
            if (reponse.IsSuccessStatusCode)
                return await reponse.Content.ReadFromJsonAsync<BookModel>();
            return null;
        }
    }
}