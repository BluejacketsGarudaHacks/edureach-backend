namespace Backend.Dtos;

public class CommunityRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid LocationId { get; set; }
}