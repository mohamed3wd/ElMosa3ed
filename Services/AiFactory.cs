namespace ElMosa3ed.Api.Services;
public class AiFactory
{
    private readonly GeminiService _gemini;
    private readonly OpenAiService _openAi;
    public AiFactory(GeminiService gemini, OpenAiService openAi) { _gemini = gemini; _openAi = openAi; }
    public IAiService GetService(string plan) { return plan == "Pro" ? _openAi : _gemini; }
}