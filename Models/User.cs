using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Fullname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public bool IsVolunteer { get; set; } = false;

        [Required]
        public DateOnly Dob { get; set; }

        public string ImagePath { get; set; } = string.Empty;
        
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<CommunityMember> Members{ get; set; }
        public ICollection<Notification> Notifications{ get; set; }
        public ICollection<Feedback> GivenFeedbacks{ get; set; }
        public ICollection<Feedback> ReceivedFeedbacks{ get; set; }
        public ICollection<UserSummary> UserSummaries{ get; set; }
    }
}   
