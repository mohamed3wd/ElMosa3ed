using Microsoft.EntityFrameworkCore;
using ElMosa3ed.Api.Models;
namespace ElMosa3ed.Api.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UsageLog> UsageLogs { get; set; }
    public DbSet<ChatHistory> ChatHistories { get; set; }
}