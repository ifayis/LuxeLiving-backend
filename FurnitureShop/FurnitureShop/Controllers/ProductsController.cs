using FurnitureShop.API.Common;
using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(
            IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            return Ok(await _productService.GetActiveAsync());
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAllAsync());
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ProductNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var product = await _productService.GetBySlugAsync(slug);

            if (product == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ProductNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            return Ok(
                await _productService.GetByCategoryAsync(categoryId));
        }

        [AllowAnonymous]
        [HttpGet("featured")]
        public async Task<IActionResult> Featured()
        {
            return Ok(
                await _productService.GetFeaturedProductsAsync());
        }

        [AllowAnonymous]
        [HttpGet("new-arrivals")]
        public async Task<IActionResult> NewArrivals()
        {
            return Ok(
                await _productService.GetNewArrivalProductsAsync());
        }

        [AllowAnonymous]
        [HttpGet("best-sellers")]
        public async Task<IActionResult> BestSellers()
        {
            return Ok(
                await _productService.GetBestSellerProductsAsync());
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword)
        {
            return Ok(
                await _productService.SearchAsync(keyword));
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProductRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            var product =
                await _productService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateProductRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            return Ok(
                await _productService.UpdateAsync(
                    id,
                    request));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("{id:guid}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _productService.ActivateAsync(id);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.ProductActivated));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _productService.DeactivateAsync(id);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.ProductDeactivated));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.ProductDeleted));
        }
    }
}