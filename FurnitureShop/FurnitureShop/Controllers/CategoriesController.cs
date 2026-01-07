using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;

        public CategoriesController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequestDto request)
        {
            var result = await _CategoryService.CreateAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _CategoryService.GetAllAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _CategoryService.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateCategoryRequestDto request)
        {
            var updated = await _CategoryService.UpdateAsync(id, request);

            if (!updated)
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
                    ResponseMessages.Success
                )
            );
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _CategoryService.DeleteByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var response = await _CategoryService.DeleteAllAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
