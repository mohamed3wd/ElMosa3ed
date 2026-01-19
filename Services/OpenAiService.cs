using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
namespace ElMosa3ed.Api.Services;
public class OpenAiService : IAiService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    public OpenAiService(HttpClient http, IConfiguration config) { _http = http; _config = config; }
    public async Task<string> Ask(string prompt)
    {
        var apiKey = _config["OpenAi:Key"];
        if(string.IsNullOrEmpty(apiKey)) return "Server Error: OpenAI Key missing (Pro Plan).";
        var req = new { model = "gpt-4o-mini", messages = new[] { new { role = "user", content = prompt } } };
        var jsonContent = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        requestMessage.Content = jsonContent;
        var res = await _http.SendAsync(requestMessage);
        res.EnsureSuccessStatusCode();
        var responseString = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseString);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "";
    }
}