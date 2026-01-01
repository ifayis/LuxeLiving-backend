using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequestDto request)
        {
            await _categoryService.CreateAsync(request);
            return Ok("Category created successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var deleted = await _categoryService.DeleteByIdAsync(id);
            if (!deleted)
                return NotFound("Category not found");

            return Ok("Category deleted successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await _categoryService.DeleteAllAsync();
            return Ok("All categories deleted successfully");
        }
    }

}