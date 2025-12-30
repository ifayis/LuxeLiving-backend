using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequestDto request)
        {
            await _productService.CreateAsync(request);
            return Ok("Product created successfully");
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(ApiResponse<string>.Fail("Product not found", 404));

            return Ok(ApiResponse<ProductResponseDto>.Success(product));
        }

        [HttpGet("category/{categoryId:Guid}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllProducts();
            return StatusCode(result.StatusCode, result);
        }

    }
}
