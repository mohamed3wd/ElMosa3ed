using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElMosa3ed.Api.Data;
using ElMosa3ed.Api.Services;
namespace ElMosa3ed.Api.Controllers;
[ApiController]
[Route("api/usage")]
public class UsageController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly UsageService _usageService;
    public UsageController(AppDbContext db, UsageService usageService) { _db = db; _usageService = usageService; }
    [HttpGet("today/{deviceId}")]
    public async Task<IActionResult> Today(string deviceId)
    {
        var user = await _usageService.GetOrCreateUser(deviceId);
        var today = DateTime.UtcNow.Date;
        var usage = await _db.UsageLogs.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Date == today);
        return Ok(new { used = usage?.RequestCount ?? 0, limit = user.Plan == "Pro" ? "∞" : "10", plan = user.Plan });
    }
}