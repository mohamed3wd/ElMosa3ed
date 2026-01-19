using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElMosa3ed.Api.Data;
using ElMosa3ed.Api.Services;
namespace ElMosa3ed.Api.Controllers;
[ApiController]
[Route("api/history")]
public class HistoryController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly UsageService _usageService;
    public HistoryController(AppDbContext db, UsageService usageService) { _db = db; _usageService = usageService; }
    [HttpGet("{deviceId}")]
    public async Task<IActionResult> GetHistory(string deviceId)
    {
        var user = await _usageService.GetOrCreateUser(deviceId);
        var history = await _db.ChatHistories.Where(h => h.UserId == user.Id).OrderByDescending(h => h.CreatedAt).Take(50).ToListAsync();
        return Ok(history);
    }
    [HttpPost("favorite/{id}")]
    public async Task<IActionResult> ToggleFavorite(int id)
    {
        var item = await _db.ChatHistories.FindAsync(id);
        if (item == null) return NotFound();
        item.IsFavorite = !item.IsFavorite;
        await _db.SaveChangesAsync();
        return Ok();
    }
}