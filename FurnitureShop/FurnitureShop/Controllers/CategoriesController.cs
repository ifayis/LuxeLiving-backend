using FurnitureShop.Application.Common;
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
            var category = await _categoryService.CreateAsync(request);

            return Ok(
                ApiResponse<CategoryResponseDto>.Success(
                    category,
                    ResponseMessages.CategoryCreated
                )
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(
                ApiResponse<List<CategoryResponseDto>>.Success(categories)
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.NotFound,
                        404
                    )
                );
            }

            return Ok(
                ApiResponse<CategoryResponseDto>.Success(category)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var deleted = await _categoryService.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.NotFound,
                        404
                    )
                );
            }

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.CategoryDeleted
                )
            );
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> DeleteAll()
        {
            await _categoryService.DeleteAllAsync();

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.CategoriesDeleted
                )
            );
        }
    }
}
