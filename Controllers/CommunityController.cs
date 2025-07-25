using Backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;
using Backend.Shared.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly ImageUtil _imageUtil;
    private readonly CommunityRepository _repository;
    private readonly UserRepository _userRepository;

    public CommunityController(CommunityRepository repository, UserRepository userRepository, ImageUtil imageUtil)
    {
        _repository = repository;
        _imageUtil = imageUtil;
        _userRepository = userRepository;
    }

    // GET: api/community
    [HttpGet]
    public async Task<ActionResult<List<Community>>> GetAll()
    {
        var communities = await _repository.GetAllCommunitiesAsync();
        return Ok(communities);
    }

    // GET: api/community/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Community>> GetById(Guid id)
    {
        var community = await _repository.GetCommunityByIdAsync(id);
        if (community == null)
            return NotFound();

        return Ok(community);
    }

    // POST: api/community
    [HttpPost]
    public async Task<ActionResult<Community>> Create([FromForm] CommunityRequest communityRequest)
    {
        var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
        var community = await this.CreateCommunityObject(communityRequest);
        var created = await _repository.AddCommunityAsync(community);
        var communityMember = await _repository.AddCommunityMemberAsync(
            community.Id, userId, true
        );
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/community/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Community>> Update(Guid id, [FromForm] CommunityRequest communityRequest)
    {
        var updatedCommunity = await this.CreateCommunityObject(communityRequest);
        var result = await _repository.UpdateCommunityAsync(id, updatedCommunity);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // DELETE: api/community/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _repository.DeleteCommunityAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ICollection<Community>>>
        GetUserJoinedCommunities(Guid userId)
    {
        var result = await _repository.GetUserJoinedCommunitiesAsync(userId);
        return Ok(result);
    }
    
    [HttpPost("add-member")]
    public async Task<ActionResult<CommunityMember>> AddMember([FromBody] CommunityMemberRequest memberRequest)
    {
        var communityMember = await _repository.AddCommunityMemberAsync(
            memberRequest.CommunityId, memberRequest.MemberId, false);

        var user = await _userRepository.GetUserById(memberRequest.MemberId);

        var message = $"{user.FullName} Telah bergabung ke dalam komunitas";
        await _userRepository.AddUserNotificationByCommunityId(memberRequest.CommunityId, message);
        
        return Ok(communityMember);
    }

    // PUT: api/community/accept-member
    [HttpPut("accept-member")]
    public async Task<ActionResult<CommunityMember>> UpdateMember([FromBody] CommunityMemberRequest memberRequest)
    {
        var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);

        var joinedUser = await _repository.GetCommunityMemberByMemberIdAndCommunityId(memberRequest.CommunityId, userId);
        if(joinedUser == null) {
            return BadRequest("User is not a member of this community");
        }

        var communityMember = await _repository.GetCommunityMemberByMemberIdAndCommunityId(memberRequest.CommunityId, memberRequest.MemberId);
        communityMember.IsJoined = true;
        
        communityMember = await _repository.UpdateCommunityMemberAsync(communityMember);
        return Ok(communityMember);
    }

    // private Community CreateCommunityObject(CommunityRequest communityRequest)
    // {
    //     var community = new Community()
    //      {
    //          LocationId = communityRequest.LocationId,
    //          Description = communityRequest.Description,
    //          Name = communityRequest.Name,
    //          ImagePath = communityRequest.ImagePath,
    //      };

    //      return community;
    // }

    private async Task<Community> CreateCommunityObject(CommunityRequest communityRequest)
    {
        var community = new Community()
        {
            LocationId = communityRequest.LocationId,
            Description = communityRequest.Description,
            Name = communityRequest.Name,
            ImagePath = "/Images/basic_group.png"
        };

         if (communityRequest.Image != null)
            {
                 using (var memoryStream = new MemoryStream())
                {
                    await communityRequest.Image.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var imageName = _imageUtil.SaveImage(imageBytes, communityRequest.Image.FileName);
                   community.ImagePath = imageName;
                }
            }

        return community;
    }
}