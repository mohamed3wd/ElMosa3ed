using System.Text.Json;
using System.Text;
namespace ElMosa3ed.Api.Services;
public class GeminiService : IAiService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    public GeminiService(HttpClient http, IConfiguration config) { _http = http; _config = config; }
    public async Task<string> Ask(string prompt)
    {
        var apiKey = _config["Gemini:Key"];
        var model = _config["Gemini:Model"] ?? "gemini-2.0-flash";
        if(string.IsNullOrEmpty(apiKey)) return "Server Error: Gemini Key missing.";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
        var req = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
        var jsonContent = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
        var res = await _http.PostAsync(url, jsonContent);
        if (!res.IsSuccessStatusCode)
        {
            var errorContent = await res.Content.ReadAsStringAsync();
            return $"Gemini API Error ({res.StatusCode}): {errorContent}";
        }
        var responseString = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseString);
        try { return doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? ""; } catch { return "Error parsing Gemini response."; }
    }
}