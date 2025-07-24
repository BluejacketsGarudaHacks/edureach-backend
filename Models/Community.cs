using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Community
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string LocationId { get; set; } = string.Empty;

        [Required]
        public string ImagePath { get; set; } = string.Empty;
    }
}   
