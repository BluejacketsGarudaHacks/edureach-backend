using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Notification: ModelWithDate
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid UserId { get; set; } 
    [Required]
    public string Message { get; set; }
    [Required]
    public bool IsShown { get; set; } = false;
    [Required]
    public bool IsChecked { get; set; } = false;
    
    public User User { get; set; }
}