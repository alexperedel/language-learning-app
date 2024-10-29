using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LearningApp
{
    public class DictionaryService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<Words?> SearchWordAsync(string query)
        {
            string apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en/{query}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonData = await response.Content.ReadAsStringAsync();
                List<Words>? wordMeanings = JsonConvert.DeserializeObject<List<Words>>(jsonData);

                return wordMeanings?.Count > 0 ? wordMeanings[0] : null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return null;
            }
        }
    }
}
