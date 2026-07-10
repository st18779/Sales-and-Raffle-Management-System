using server.Models;
using static server.DTOs.CategoryDto;


namespace server.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> CreateCategory(Category category);
        Task<Category?> UpdateCategory(int id, Category category);
        Task<bool> DeleteCategory(int id);

    }
}
