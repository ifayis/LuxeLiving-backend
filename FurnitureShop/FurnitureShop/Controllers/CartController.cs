using FurnitureShop.API.Common;
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
    [Authorize]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                ApiResponse<object>.Fail(
                    "Validation failed",
                    ModelState.ToErrorDictionary(),
                    400
                )
            );
        }
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


        await _cartService.AddToCartAsync(userId, request);
        return Ok(ApiResponse<string>.Success("Item added to cart"));
    }

}
