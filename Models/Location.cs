using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Location : ModelWithDate
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Province {set; get; }
    [Required]
    public string City { set; get; }
    
    public ICollection<Community> Communities { set; get; }
}