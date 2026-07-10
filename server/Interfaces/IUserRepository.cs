using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersList();
        Task<User?> GetUserById(string id);
        Task<User?> GetUserByEmail(string email);
        Task<User> CreateUser(User user);
        Task<User?> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
    }
}
