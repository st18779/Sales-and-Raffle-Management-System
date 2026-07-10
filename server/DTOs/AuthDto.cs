using System.ComponentModel.DataAnnotations;
using static server.DTOs.UserDto;

namespace server.DTOs
{
    public class AuthDto
    {
        public class LoginRequestDto
        {
            [Required]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponseDto
        {
            public string Token { get; set; } = string.Empty;
            public string TokenType { get; set; } = "Bearer";
            public int ExpiresIn { get; set; }
            public UserResponseDto User { get; set; } = null!;
        }
    }
}
