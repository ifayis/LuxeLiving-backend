using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/wishlist")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToWishlistRequestDto request)
        {
            await _wishlistService.AddAsync(GetUserId(), request);
            return Ok();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var result = await _wishlistService.GetMyWishlistAsync(GetUserId());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _wishlistService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("item/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            await _wishlistService.RemoveItemAsync(GetUserId(), itemId);
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _wishlistService.ClearAsync(GetUserId());
            return NoContent();
        }
    }
}
