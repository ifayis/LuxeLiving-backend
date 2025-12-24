using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers;

[ApiController]
[Route("api/categories")]
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
        var categoryId = await _categoryService.CreateAsync(request);
        return Ok(new { categoryId });
    }
}
