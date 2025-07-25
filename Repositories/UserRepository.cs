using System.Data;
using Backend.Infrastructure.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid?> GetUserIdByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user?.Id;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            var existing = await _db.Users.FindAsync(id);
            if(existing == null)
                throw new DataException("Akun tidak ditemukan");

            existing.Email = user.Email;
            existing.FullName = user.FullName;
            existing.Dob = user.Dob;
            existing.Password = user.Password;
            existing.IsVolunteer = user.IsVolunteer;

            _db.Users.Update(existing);
            await _db.SaveChangesAsync();
            
            return existing;
        }

        public async Task InsertNewUserAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();

            return notification;
        }

        public async Task<Notification> UpdateNotificationAsync(Guid id, Notification notification)
        {
            var findNotification = await _db.Notifications.FindAsync(id);

            if (findNotification == null)
                throw new DataException("Notification not found");
            
            findNotification.IsShown = notification.IsShown;
            findNotification.IsChecked = notification.IsChecked;
            findNotification.Message = notification.Message;
            findNotification.IsChecked = notification.IsChecked;
            
            _db.Notifications.Update(findNotification);
            await _db.SaveChangesAsync();
            
            return findNotification;
        }

        public async Task<ICollection<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            var notifications = await _db.Notifications
                .Where(u => u.UserId == userId)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            
            return notifications;
        }

        public async Task<UserSummary> CreateUserSummary(UserSummary summary)
        {
            _db.UserSummaries.Add(summary);
            var result = await _db.SaveChangesAsync();

            return summary;
        }

        public async Task<ICollection<UserSummary>> GetUserSummariesAsync(Guid userId)
        {
            var summaries = await _db.UserSummaries.Where(us => us.UserId.Equals(userId))
                .ToListAsync();

            return summaries;
        }
        public async Task AddUserNotificationByCommunityId(Guid communityId, string message)
        {
            var communityMembers = await _db.CommunityMembers
                .Where(cm => cm.CommunityId == communityId)
                .ToListAsync();

            foreach (var member in communityMembers)
            {
                var notification = new Notification
                {
                    UserId = member.UserId,
                    Message = message,
                    IsShown = false,
                    CreatedAt = DateTime.UtcNow,
                    IsChecked = false,
                };

                _db.Notifications.Add(notification);
            }

            await _db.SaveChangesAsync();
        }
    }
}
