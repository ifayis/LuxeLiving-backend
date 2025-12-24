using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces;
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
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpGet("category/{categoryId:Guid}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }
    }
}
