using System.Data;
using Backend.Infrastructure.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class CommunityRepository
{
    private readonly AppDbContext _db;

    public CommunityRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Community> AddCommunityAsync(Community community)
    {
        _db.Communities.Add(community);
        await _db.SaveChangesAsync();

        return community;
    }


    // Read (by ID)
    public async Task<Community?> GetCommunityByIdAsync(Guid id)
    {
        return await _db.Communities
            .Include(c => c.Members) // optional, include related entities
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // Read (all)
    public async Task<List<Community>> GetAllCommunitiesAsync()
    {
        return await _db.Communities
            .Include(c => c.Members)
            .ThenInclude(m => m.User)
            .Include(c => c.Schedules)
            .Include(c => c.Location)
            .ToListAsync();
    }

    // Update
    public async Task<Community?> UpdateCommunityAsync(Guid id, Community updatedCommunity)
    {
        var existing = await _db.Communities.FindAsync(id);
        if (existing == null)
            return null;

        // Update fields
        existing.Name = updatedCommunity.Name;
        existing.Description = updatedCommunity.Description;
        existing.LocationId = updatedCommunity.LocationId;

        _db.Communities.Update(existing);
        await _db.SaveChangesAsync();
        return existing;
    }

    // Delete
    public async Task<bool> DeleteCommunityAsync(Guid id)
    {
        var existing = await _db.Communities.FindAsync(id);
        if (existing == null)
            return false;

        _db.Communities.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<CommunityMember> AddCommunityMemberAsync(Guid communityId, Guid memberId, bool IsJoined)
    {
        var community = await _db.Communities.
            Where(c => c.Id.Equals(communityId))
            .FirstOrDefaultAsync();

        if (community == null)
            throw new DataException("Community tidak ditemukan");
        
        var user = await _db.Users.FindAsync(memberId);
        
        if(user == null)
            throw new DataException("User tidak ditemukan");

        var communityMember = await _db.CommunityMembers.FirstOrDefaultAsync(cm => cm.UserId == memberId && cm.CommunityId == communityId);
        if(communityMember != null)
            throw new DataException("User already joined this community");
        

        communityMember = new CommunityMember()
        {
            UserId = memberId,
            CommunityId = communityId,
            IsJoined = IsJoined
        };

        _db.CommunityMembers.Add(communityMember);
        await _db.SaveChangesAsync();
        return communityMember;
    }

    public async Task<CommunityMember> GetCommunityMemberByMemberIdAndCommunityId(Guid communityId, Guid memberId)
    {
        return await _db.CommunityMembers.FirstOrDefaultAsync(cm => cm.UserId == memberId && cm.CommunityId == communityId);
    }

    public async Task<CommunityMember> UpdateCommunityMemberAsync(CommunityMember communityMember)
    {
        _db.CommunityMembers.Update(communityMember);
        await _db.SaveChangesAsync();
        return communityMember;
    }
}
