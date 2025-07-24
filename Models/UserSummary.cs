using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class UserSummary
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string SummaryTitle { get; set; }
    
    [Required]
    public string SummaryResult { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User;
}
    