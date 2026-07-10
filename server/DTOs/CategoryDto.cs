using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class CategoryDto
    {
        public class CategoryResponseDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class CategoryCreateDto
        {
            [Required]
            public string Name { get; set; } = string.Empty;
        }

    }
}
