using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ChineseSaleContext _context;

        public UserRepository(ChineseSaleContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetUsersList()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserById(string id)
        {
            return await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users
                            .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUser(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) 
                return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
