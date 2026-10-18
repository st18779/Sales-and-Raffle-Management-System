using server.Models;
using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class GiftDTOs
    {
        public class GiftResponseDto
        {
            public int Id { get; set; }
            public string Name { get; set; }=string.Empty;
            public string Description { get; set; } = string.Empty;
            public int Price { get; set; }
            public bool IsRandomized { get; set; } = false;
            public string CategoryName { get; set; } = string.Empty;
            public string DonorName { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public WinnerDto? WinnerUser { get; set; } = null;

        }

        // Public info about a raffle winner - never expose the full User entity (it contains the password)
        public class WinnerDto
        {
            public string FullName { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;

            public static WinnerDto? From(User? user) =>
                user == null ? null : new WinnerDto { FullName = user.FullName, Phone = user.Phone };
        }

        public class GiftCreateDto
        {
            //[Required]
            //public int Id { get; set; }
            [Required]
            [MaxLength(20)]
            public string Name { get; set; } = string.Empty;
            [Required]
            [MaxLength(50)]
            public string Description { get; set; } = string.Empty;
            [Required]
            [AllowedValues(10,50,ErrorMessage = "A ticket price can be only 10 or 50")]
            public int Price { get; set; }
            [Required]
            public int CategoryId { get; set; }
            [Required]
            public string? DonorId { get; set; } = string.Empty;
            [Required]
            public string? ImageUrl { get; set; }

        }

        public class GiftUpdateDto
        {
            
            [MaxLength(20)]
            public string Name { get; set; } = string.Empty;
            
            [MaxLength(50)]
            public string Description { get; set; } = string.Empty;
            
            [AllowedValues(10, 50, ErrorMessage = "A ticket price can be only 10 or 50")]
            public int? Price { get; set; }
            
            public int? CategoryId { get; set; }
            
            public string? DonorId { get; set; }
            
            public string? ImageUrl { get; set; }

        }
    }
}
