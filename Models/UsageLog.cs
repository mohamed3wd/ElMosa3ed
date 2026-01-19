using System.ComponentModel.DataAnnotations;
namespace ElMosa3ed.Api.Models;
public class UsageLog
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public int RequestCount { get; set; }
}