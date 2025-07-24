using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Schedule : ModelWithDate
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public DateTime ScheduleTime { get; set; }
    [Required]
    public Guid VolunteerId { get; set; }
    [Required]
    public Guid CommunityId { get; set; }
    
    public User Volunteer { get; set; }
    public Community Community { get; set; }
}
    