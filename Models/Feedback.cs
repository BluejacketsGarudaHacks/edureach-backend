using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Feedback: ModelWithDate
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid VolunteerId { get; set; }
    [Required]
    public string Message { get; set; }
    
    public User User { get; set; }
    public User Volunteer { get; set; }
}