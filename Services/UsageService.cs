using ElMosa3ed.Api.Data;
using ElMosa3ed.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace ElMosa3ed.Api.Services;
public class UsageService
{
    private readonly AppDbContext _db;
    public UsageService(AppDbContext db) { _db = db; }
    public async Task<AppUser> GetOrCreateUser(string deviceId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.DeviceId == deviceId);
        if (user == null) { user = new AppUser { DeviceId = deviceId }; _db.Users.Add(user); await _db.SaveChangesAsync(); }
        return user;
    }
    public async Task<bool> CanUse(AppUser user)
    {
        if (user.Plan == "Pro") {
            if (user.ProExpiryDate.HasValue && user.ProExpiryDate > DateTime.UtcNow) return true;
            user.Plan = "Free"; await _db.SaveChangesAsync();
        }
        var today = DateTime.UtcNow.Date;
        var usage = await _db.UsageLogs.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Date == today);
        return usage == null || usage.RequestCount < 10;
    }
    public async Task Increment(AppUser user)
    {
        var today = DateTime.UtcNow.Date;
        var usage = await _db.UsageLogs.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Date == today);
        if (usage == null) { usage = new UsageLog { UserId = user.Id, Date = today, RequestCount = 1 }; _db.UsageLogs.Add(usage); }
        else { usage.RequestCount++; }
        await _db.SaveChangesAsync();
    }
}