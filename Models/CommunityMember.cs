using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class CommunityMember: ModelWithDate
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid CommunityId { get; set; }

    public User User { get; set; }
    public Community Commuity { get; set; }
}