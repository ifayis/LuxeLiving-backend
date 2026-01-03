using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Services;
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

            return Ok(
                ApiResponse<ProductResponseDto>.Success(
                    product,
                    ResponseMessages.ProductCreated
                )
            );
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.NotFound,
                        404
                    )
                );
            }

            return Ok(
                ApiResponse<ProductResponseDto>.Success(product)
            );
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);

            return Ok(
                ApiResponse<List<ProductResponseDto>>.Success(products)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();

            return Ok(
                ApiResponse<List<ProductResponseDto>>.Success(products)
            );
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
    }
}
