using System.ComponentModel.DataAnnotations;
namespace ElMosa3ed.Api.Models;
public class ChatHistory
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public bool IsFavorite { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}