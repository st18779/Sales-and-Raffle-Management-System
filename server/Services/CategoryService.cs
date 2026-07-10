using server.Interfaces;
using server.Models;
using server.Repositories;
using static server.DTOs.CategoryDto;
using static server.DTOs.DonorDto;

namespace server.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDto?> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return category != null ? MapToResponseDto(category) : null;
        }

        private static CategoryResponseDto MapToResponseDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return categories.Select(MapToResponseDto);
        }

        public async Task<CategoryResponseDto> CreateCategory(CategoryCreateDto category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
            };
            var createdCategory = await _categoryRepository.CreateCategory(newCategory);
            return MapToResponseDto(newCategory);
        }

        public async Task<CategoryResponseDto?> UpdateCategory(int id, CategoryCreateDto category)
        {
            var existCategory = await _categoryRepository.GetCategoryById(id);
            if (existCategory == null)
                return null;
            if (category.Name != null) existCategory.Name = category.Name;

            var updateCategory = await _categoryRepository.UpdateCategory(id, existCategory);
            return updateCategory != null ? MapToResponseDto(updateCategory) : null;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            return await _categoryRepository.DeleteCategory(id);
        }
    }
}
