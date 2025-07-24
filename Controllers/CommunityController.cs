using Backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly CommunityRepository _repository;

    public CommunityController(CommunityRepository repository)
    {
        _repository = repository;
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
    public async Task<ActionResult<Community>> Create([FromBody] CommunityRequest communityRequest)
    {
        var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
        var community = this.CreateCommunityObject(communityRequest);
        var created = await _repository.AddCommunityAsync(community);
        var communityMember = await _repository.AddCommunityMemberAsync(
            community.Id, userId
        );
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/community/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Community>> Update(Guid id, [FromBody] CommunityRequest communityRequest)
    {
        var updatedCommunity = this.CreateCommunityObject(communityRequest);
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

    [HttpPost("add-member")]
    public async Task<ActionResult<CommunityMember>> AddMember([FromBody] CommunityMemberRequest memberRequest)
    {
        var communityMember = await _repository.AddCommunityMemberAsync(
            memberRequest.CommunityId, memberRequest.MemberId);
        
        return Ok(communityMember);
    }

    private Community CreateCommunityObject(CommunityRequest communityRequest)
    {
        var community = new Community()
         {
             LocationId = communityRequest.LocationId,
             Description = communityRequest.Description,
             Name = communityRequest.Name,
         };

         return community;
    }
}