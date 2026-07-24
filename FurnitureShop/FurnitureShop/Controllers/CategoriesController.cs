using FurnitureShop.API.Common;
using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive()
        {
            return Ok(await _categoryService.GetActiveAsync());
        }

        [AllowAnonymous]
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.CategoryNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(category);
        }

        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var category = await _categoryService.GetBySlugAsync(slug);

            if (category == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.CategoryNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(category);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            var category = await _categoryService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                category);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateCategoryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            return Ok(await _categoryService.UpdateAsync(id, request));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok(ResponseMessages.CategoryDeleted);
        }
    }
}