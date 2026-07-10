using server.Models;
using static server.DTOs.CategoryDto;

namespace server.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategories();
        Task<CategoryResponseDto?> GetCategoryById(int id);
        Task<CategoryResponseDto> CreateCategory(CategoryCreateDto category);
        Task<CategoryResponseDto> UpdateCategory(int id, CategoryCreateDto category);
        Task<bool> DeleteCategory(int id);
    }
}
