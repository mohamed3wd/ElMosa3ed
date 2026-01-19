using System.ComponentModel.DataAnnotations;
namespace ElMosa3ed.Api.Models;
public class AppUser
{
    [Key]
    public int Id { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string Plan { get; set; } = "Free";
    public DateTime? ProExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}