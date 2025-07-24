using Backend.Models;
using Backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Repositories;

public class LocationRepository
{
    private readonly AppDbContext _db;

    public LocationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Location>> GetAllLocationAsync() {
        return await _db.Locations.ToListAsync();
    }
}