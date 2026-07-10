using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class DonorDto
    {
        public class DonorResponseDto
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public IEnumerable<GiftDTOs.GiftResponseDto> Gifts { get; set; } = new List<GiftDTOs.GiftResponseDto>();
        }

        public class DonorCreateDto
        {
            [Required]
            public string Id { get; set; } = string.Empty;
            [Required]
            [MaxLength(20)]
            public string Name { get; set; } = string.Empty;
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

        public class DonorUpdateDto
        {
            [MaxLength(20)]
            public string Name { get; set; } = string.Empty;
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

    }
}
