using MCC.Korsini.Announcements.Business.Abstract.SummarizeTexAbstract;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MCC.Korsini.Announcements.Business.Concrete.SummarizeTextConcrete
{
    public class OpenAIClient_Manager : IOpenAIClient_Service
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIClient_Manager(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];

            // User-Agent başlığını burada ayarlayın
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MCCKorsini_AnnouncementApp/1.0");

            // Authorization başlığını da burada ayarlayın
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> SummarizeTextAsync(string text)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Please summarize the following text in a concise way." },
                    new { role = "user", content = text }
                },
                max_tokens = 100
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
            string summary = jsonResponse.choices[0].message.content;

            return summary;
        }
    }
}
