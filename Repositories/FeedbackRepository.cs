using Backend.Infrastructure.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class FeedbackRepository
{
    private readonly AppDbContext _db;

    public FeedbackRepository(AppDbContext db)
    {
        _db = db;
    }
    
    // Create
    public async Task<Feedback> AddFeedbackAsync(Feedback feedback)
    {
        _db.Feedbacks.Add(feedback);
        await _db.SaveChangesAsync();
        return feedback;
    }

    // Read - Get all
    public async Task<List<Feedback>> GetAllFeedbacksAsync()
    {
        return await _db.Feedbacks
            .Include(f => f.User)
            .Include(f => f.Volunteer)
            .ToListAsync();
    }

    // Read - Get by ID
    public async Task<Feedback?> GetFeedbackByIdAsync(Guid id)
    {
        return await _db.Feedbacks
            .Include(f => f.User)
            .Include(f => f.Volunteer)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    // Update
    public async Task<bool> UpdateFeedbackAsync(Guid id, Feedback feedback)
    {
        var existing = await _db.Feedbacks.FindAsync(feedback.Id);
        if (existing == null) return false;

        _db.Entry(existing).CurrentValues.SetValues(feedback);
        await _db.SaveChangesAsync();
        return true;
    }

    // Delete
    public async Task<bool> DeleteFeedbackAsync(Guid id)
    {
        var feedback = await _db.Feedbacks.FindAsync(id);
        if (feedback == null) return false;

        _db.Feedbacks.Remove(feedback);
        await _db.SaveChangesAsync();
        return true;
    }
}