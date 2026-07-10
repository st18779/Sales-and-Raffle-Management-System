using server.Models;
using static server.DTOs.AuthDto;
using static server.DTOs.UserDto;

namespace server.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetUsersList();
        Task<UserResponseDto?> GetUserById(string id);
        Task<UserResponseDto?> GetUserByEmail(string email);
        Task<UserResponseDto> CreateUser(UserCreateDto user);
        Task<UserResponseDto?> UpdateUser(string id, UserUpdateDto updateUser);
        Task<bool> DeleteUser(string id);
        Task<LoginResponseDto?> Authenticate(string id, string password);

    }
}
