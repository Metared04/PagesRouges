using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.API
{
    public class RandomUserService
    {
        private readonly HttpClient _httpClient;

        public RandomUserService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<RandomUserResponse> GetUsersAsync(int count = 10, string nat = "fr")
        {
            string url = $"https://randomuser.me/api/?results={count}&nat={nat}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<RandomUserResponse>(json);
            return data;
        }
    }
}
