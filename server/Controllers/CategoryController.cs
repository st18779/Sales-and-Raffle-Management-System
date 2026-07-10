using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;
using static server.DTOs.CategoryDto;

namespace server.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("categoriesList")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} does not exist!" });
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryCreateDto newCategory)
        {
            try
            {
                var category = await _categoryService.CreateCategory(newCategory);
                if (category == null)
                    return BadRequest(new { message = $"Category with name {newCategory.Name} already exists!" });
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryCreateDto category)
        {
            var result = await _categoryService.UpdateCategory(id, category);
            if (result == null)
            {
                return NotFound(new { message = $"Category with ID {id} does not exist!" });
            }
            return Ok(new { message = "Category updated successfully!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (!result)
            {
                return NotFound(new { message = $"Category with ID {id} does not exist!" });
            }
            return Ok(new { message = "Category deleted successfully!" });
        }

    }
}
