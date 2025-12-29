using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers;

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

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(AddToCartRequestDto request)
    {
        var userId = GetUserId();
        var result = await _cartService.AddToCartAsync(userId, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyCart()
    {
        var userId = GetUserId();
        var result = await _cartService.GetMyCartAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateItem(UpdateCartItemDto request)
    {
        var userId = GetUserId();
        var result = await _cartService.UpdateItemAsync(userId, request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("remove/{itemId}")]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        var userId = GetUserId();
        var result = await _cartService.RemoveItemAsync(userId, itemId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();
        var result = await _cartService.ClearCartAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
