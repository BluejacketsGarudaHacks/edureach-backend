using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class UserSummary
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid PdfPath { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }

    public User user;
}
    