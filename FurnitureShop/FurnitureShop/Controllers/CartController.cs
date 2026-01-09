using FurnitureShop.Application.common;
using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User id not found in token");

            return Guid.Parse(userId);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToCartRequestDto request)
        {
            var result = await _cartService.AddToCartAsync(GetUserId(), request);

            return Ok(result);
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyCart()
        {
            var cart = await _cartService.GetMyCartAsync(GetUserId());

            return Ok(cart);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetUserCart(Guid userId)
        {
            var cart = await _cartService.GetMyCartAsync(userId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }


        [Authorize(Roles = Roles.User)]
        [HttpDelete("remove/{productId:guid}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            await _cartService.RemoveItemAsync(GetUserId(), productId);

            return Ok();
        }


        [Authorize(Roles = Roles.User)]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateItem(UpdateCartItemRequestDto request)
        {
            var updated = await _cartService.UpdateItemAsync(GetUserId(), request);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [Authorize(Roles = Roles.User)]
        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync(GetUserId());

            return Ok();
        }
    }
}
