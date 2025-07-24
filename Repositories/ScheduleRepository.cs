using System.Data;
using Backend.Infrastructure.Database;
using Backend.Models; // adjust namespace if needed
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ScheduleRepository
{
    private readonly AppDbContext _db;

    public ScheduleRepository(AppDbContext db)
    {
        _db = db;
    }

    // Create
    public async Task<Schedule> AddScheduleAsync(Schedule schedule)
    {
        _db.Schedules.Add(schedule);
        await _db.SaveChangesAsync();
        return schedule;
    }

    // Read - Get all
    public async Task<List<Schedule>> GetAllSchedulesAsync()
    {
        return await _db.Schedules.ToListAsync();
    }

    // Read - Get by ID
    public async Task<Schedule?> GetScheduleByIdAsync(Guid id)
    {
        return await _db.Schedules.FindAsync(id);
    }

    // Update
    public async Task<bool> UpdateScheduleAsync(Guid id, Schedule schedule)
    {
        var existing = await _db.Schedules.FindAsync(id);
        if (existing == null) throw new DataException("Schedule tidak ditemukan");

        existing.CommunityId = schedule.CommunityId;
        existing.ScheduleTime = schedule.ScheduleTime;

        _db.Schedules.Update(existing);
        await _db.SaveChangesAsync();
        return true;
    }

    // Delete
    public async Task<bool> DeleteScheduleAsync(Guid id)
    {
        var schedule = await _db.Schedules.FindAsync(id);
        if (schedule == null) throw new DataException("Schedule tidak ditemukan"); 

        _db.Schedules.Remove(schedule);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<ICollection<Schedule>> GetScheduleByCommunityAsync(Guid communityId)
    {
        var schedules = _db.Schedules
            .Where(s => s.CommunityId.Equals(communityId))
            .Include(s => s.Community)
            .Include(s => s.Volunteer)
            .ToList();

        return schedules;
    }
}