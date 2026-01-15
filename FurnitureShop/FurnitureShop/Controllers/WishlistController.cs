using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/wishlist")]
    [Authorize(Roles = Roles.User)]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _WishlistService;

        public WishlistController(IWishlistService WishlistService)
        {
            _WishlistService = WishlistService;
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
        public async Task<IActionResult> Add(AddToWishlistRequestDto request)
        {
           var result =await _WishlistService.AddAsync(GetUserId(), request);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var wishlist = await _WishlistService.GetMyAsync(GetUserId());

            return Ok(wishlist);
        }

        [HttpDelete("item/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            await _WishlistService.RemoveItemAsync(GetUserId(), itemId);

            return Ok(ApiResponse<object>.Success(
                 null,
                 ResponseMessages.WishlistItemRemoved
                 )
            );
        }

        [HttpPost("move-to-cart/{ItemId:guid}")]
        public async Task<IActionResult> MoveToCart(Guid ItemId)
        {
            await _WishlistService.MoveToCartAsync(GetUserId(), ItemId);

            return Ok(
                 ApiResponse<object>.Success(
                 null,
                 ResponseMessages.WishlistItemsMoved
                 )
             );
        }


        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _WishlistService.ClearAsync(GetUserId());

            return Ok(
                ApiResponse<object>.Success(
                null,
                ResponseMessages.WishlistCleared
                )
            );
        }
    }
}
