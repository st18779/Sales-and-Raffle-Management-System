using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;
using System.Drawing;
using static server.DTOs.CategoryDto;

namespace server.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ChineseSaleContext _context;
        public CategoryRepository(ChineseSaleContext context)
        {
            _context = context;
        }
        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> CreateCategory(Category category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return newCategory;
        }

        public async Task<Category?> UpdateCategory(int id, Category category)
        {
            var existCategory = await _context.Categories.FindAsync(category.Id);
            if (existCategory == null)
                return null;
            existCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return existCategory;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
