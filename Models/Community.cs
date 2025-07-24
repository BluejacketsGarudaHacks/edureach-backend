using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Community: ModelWithDate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid LocationId { get; set; }

        [Required]
        public string ImagePath { get; set; } = string.Empty;
        
        public Location Location { get; set; }
        public ICollection<CommunityMember> Members { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}   
