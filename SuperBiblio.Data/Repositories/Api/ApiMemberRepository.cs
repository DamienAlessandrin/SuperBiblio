using SuperBiblio.Data.Models;
using System.Net.Http.Json;

namespace SuperBiblio.Data.Repositories.Api
{
    public class ApiMemberRepository : IMemberRepository
    {
        private readonly string url;
        private readonly HttpClient client;

        public ApiMemberRepository(string url)
        {
            this.url = url;
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }

        public async Task<MemberModel?> Create(MemberModel model)
        {
            var response = await client.PostAsJsonAsync($"{url}member", model);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<MemberModel>();
            return null;
        }

        public async Task<IEnumerable<MemberModel>> Get()
        {
            var response = await client.GetAsync($"{url}member");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<MemberModel>>();
            return null;
        }

        public async Task<MemberModel?> Get(int id)
        {
            var response = await client.GetAsync($"{url}member/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<MemberModel>();
            return null;
        }


    }
}
