using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces.Services;
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

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToCartRequestDto request)
        {
            var result = await _cartService.AddToCartAsync(GetUserId(), request);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyCart()
        {
            var cart = await _cartService.GetMyCartAsync(GetUserId());

            return Ok(cart);
        }

        [HttpGet("{cartId:guid}")]
        public async Task<IActionResult> GetById(Guid cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpDelete("remove/{productId:guid}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            await _cartService.RemoveItemAsync(GetUserId(), productId);

            return Ok();
        }

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


        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync(GetUserId());

            return Ok();
        }
    }
}
