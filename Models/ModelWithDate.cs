using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class ModelWithDate
{
    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}