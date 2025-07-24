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
            findNotification.Message = notification.Message;
            
            _db.Notifications.Update(findNotification);
            await _db.SaveChangesAsync();
            
            return notification;
        }

        public async Task<ICollection<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            var notifications = _db.Notifications
                .Where(u => u.UserId.Equals(userId))
                .OrderByDescending(u => u.CreatedAt)
                .ToList();
            
            return notifications;
        }
    }
}
