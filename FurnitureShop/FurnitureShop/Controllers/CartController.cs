using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        // POST
        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToCartRequestDto request)
        {
            await _cartService.AddToCartAsync(GetUserId(), request);
            return Ok("Item added to cart");
        }

        // GET ALL
        [HttpGet("my")]
        public async Task<IActionResult> GetMyCart()
        {
            var cart = await _cartService.GetMyCartAsync(GetUserId());
            return Ok(cart);
        }

        // GET BY ID
        [HttpGet("{cartId:guid}")]
        public async Task<IActionResult> GetById(Guid cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);
            return cart == null ? NotFound() : Ok(cart);
        }

        // DELETE ITEM
        [HttpDelete("remove/{productId:guid}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            await _cartService.RemoveItemAsync(GetUserId(), productId);
            return Ok("Item removed");
        }

        // CLEAR ALL
        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync(GetUserId());
            return Ok("Cart cleared");
        }
    }
}
