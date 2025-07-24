using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
   
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
} 