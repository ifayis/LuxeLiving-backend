using FurnitureShop.API.Common;
using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    [Produces("application/json")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue("UID");

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException(ErrorMessages.InvalidToken);

            return Guid.Parse(userId);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost("items")]
        public async Task<IActionResult> AddToCart(
            [FromBody] AddToCartRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            var response = await _cartService.AddToCartAsync(
                GetCurrentUserId(),
                request);

            return Ok(response);
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var cart = await _cartService.GetMyCartAsync(
                GetCurrentUserId());

            return Ok(cart);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("users/{userId:guid}")]
        public async Task<IActionResult> GetUserCart(Guid userId)
        {
            var cart = await _cartService.GetUserCartAsync(userId);

            if (cart == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.CartNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(cart);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut("items")]
        public async Task<IActionResult> UpdateCartItem(
            [FromBody] UpdateCartItemRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            var updated = await _cartService.UpdateItemAsync(
                GetCurrentUserId(),
                request);

            if (!updated)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.CartItemNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(ResponseMessages.CartUpdated);
        }

        [Authorize(Roles = Roles.User)]
        [HttpDelete("items/{productId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            await _cartService.RemoveItemAsync(
                GetCurrentUserId(),
                productId);

            return Ok(ResponseMessages.CartRemoved);
        }

        [Authorize(Roles = Roles.User)]
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync(
                GetCurrentUserId());

            return Ok(ResponseMessages.CartCleared);
        }
    }
}