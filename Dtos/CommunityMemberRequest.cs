namespace Backend.Dtos;

public class CommunityMemberRequest
{
    public Guid CommunityId { get; set; }
    public Guid MemberId { get; set; }
}