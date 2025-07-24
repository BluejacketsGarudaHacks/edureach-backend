using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMembers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Notification> Notifications{ get; set; }
 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            
            modelBuilder.Entity<User>()
                .HasMany<Schedule>(u => u.Schedules)
                .WithOne(s => s.Volunteer)
                .HasForeignKey(s => s.VolunteerId)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                            .HasMany<Notification>(u => u.Notifications)
                            .WithOne(s => s.User)
                            .HasForeignKey(s => s.UserId)
                            .IsRequired();
            
            modelBuilder.Entity<User>()
                .HasMany<CommunityMember>(u => u.Members)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .IsRequired();

            modelBuilder.Entity<Community>()
                .HasKey(c => c.Id);
            
            modelBuilder.Entity<Community>()
                            .HasMany<CommunityMember>(u => u.Members)
                            .WithOne(s => s.Commuity)
                            .HasForeignKey(s => s.CommunityId)
                            .IsRequired();
            
            modelBuilder.Entity<Community>()
                .HasMany<Schedule>(u => u.Schedules)
                .WithOne(s => s.Community)
                .HasForeignKey(s => s.CommunityId)
                .IsRequired();

            modelBuilder.Entity<Location>()
                .HasKey(l => l.Id);
            
            modelBuilder.Entity<Location>()
                .HasMany<Community>(u => u.Communities)
                .WithOne(s => s.Location)
                .HasForeignKey(s => s.LocationId)
                .IsRequired();
        }
    }
} 

