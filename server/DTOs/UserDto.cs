using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class UserDto
    {
        public class UserResponseDto
        {
            public string Id { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public bool IsAdmin { get; set; }
        }

        public class UserCreateDto
        {
            [Required, RegularExpression(@"^\d{9}$", ErrorMessage = "Id must have 9 numbers!")]
            public string Id { get; set; } = string.Empty;
            [Required,MaxLength(20)]
            public string FullName { get; set; } = string.Empty;
            [Required,EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required,Phone]
            public string Phone { get; set; } = string.Empty;
            [Required]
            [MinLength(8, ErrorMessage = "Password must have at least 8 chars!")]
            public string Password { get; set; } = string.Empty;
        }
        public class UserUpdateDto
        {
            [MaxLength(20)]
            public string FullName { get; set; } = string.Empty;
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required, Phone]
            public string Phone { get; set; } = string.Empty;
        }
        
        public class UserWithSumTicketsDto
        {
            public string Id { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public int NumOfTickets { get; set; }
        }

    }
}
