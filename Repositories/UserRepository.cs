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
    }
}
