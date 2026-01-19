using Microsoft.AspNetCore.Mvc;
using ElMosa3ed.Api.Services;
using ElMosa3ed.Api.Models;
using ElMosa3ed.Api.Data;
namespace ElMosa3ed.Api.Controllers;
[ApiController]
[Route("api/ask")]
public class AiController : ControllerBase
{
    private readonly AiFactory _aiFactory;
    private readonly UsageService _usageService;
    private readonly AppDbContext _db;
    public AiController(AiFactory aiFactory, UsageService usageService, AppDbContext db) { _aiFactory = aiFactory; _usageService = usageService; _db = db; }
    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] AskDto dto, [FromHeader(Name = "X-Device-Id")] string? deviceId)
    {
        if (string.IsNullOrEmpty(deviceId)) return BadRequest("Device ID is required");
        var user = await _usageService.GetOrCreateUser(deviceId);
        if (!await _usageService.CanUse(user)) return StatusCode(429, new { message = "لقد استهلكت رصيدك اليومي (10 محاولات). اشترك في Pro للمزيد!" });
        var aiService = _aiFactory.GetService(user.Plan);
        var result = await aiService.Ask(dto.Prompt);
        var history = new ChatHistory { UserId = user.Id, Prompt = dto.Prompt, Answer = result, CreatedAt = DateTime.UtcNow };
        _db.ChatHistories.Add(history);
        await _usageService.Increment(user);
        await _db.SaveChangesAsync();
        return Ok(new { answer = result });
    }
}