using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("Add")]
        public async Task<IActionResult> Create(CreateProductRequestDto request)
        {
            var product = await _productService.CreateAsync(request);

            return Ok(product);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("single/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);

            return Ok(products);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("Update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateProductRequestDto request)
        {
            var updated = await _productService.UpdateAsync(id, request);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("Deactivate/{id:guid}")]
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

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("Activate/{id:guid}")]
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

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("Delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _productService.DeleteByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("Clear")]
        public async Task<IActionResult> Clear()
        {
            var response = await _productService.DeleteAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Only jpg, jpeg, png and webp files are allowed.");
            }

            const long maxFileSize = 5 * 1024 * 1024;

            if (image.Length > maxFileSize)
            {
                return BadRequest("Maximum file size is 5 MB.");
            }

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "products");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await image.CopyToAsync(stream);

            var imageUrl = $"/uploads/products/{fileName}";

            return Ok(new UploadProductImageResponseDto
            {
                ImageUrl = imageUrl
            });
        }
    }
}
