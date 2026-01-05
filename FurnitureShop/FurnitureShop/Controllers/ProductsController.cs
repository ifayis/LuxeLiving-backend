using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> Create(CreateProductRequestDto request)
        {
            var product = await _productService.CreateAsync(request);

            return Ok(product);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();

            return Ok(products);
        }

        [HttpPut("activate/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _productService.ActivateProductAsync(id);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.ProductActivated
                )
            );
        }

        [HttpPut("deactivate/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _productService.DeactivateProductAsync(id);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.ProductDeactivated
                )
            );
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _productService.DeleteByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var response = await _productService.DeleteAllAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
