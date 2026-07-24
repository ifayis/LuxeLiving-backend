using FurnitureShop.API.Common;
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
    [Produces("application/json")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(
            IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirstValue("UID");

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException();

            return Guid.Parse(userId);
        }


        [HttpPost]
        [ProducesResponseType(
            typeof(WishlistResponseDto),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(
            [FromBody] AddToWishlistRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            var wishlist = await _wishlistService
                .AddAsync(GetUserId(), request);

            return Ok(wishlist);
        }

        [HttpGet]
        [ProducesResponseType(
            typeof(WishlistResponseDto),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyWishlist()
        {
            var wishlist = await _wishlistService
                .GetMyWishlistAsync(GetUserId());

            return Ok(wishlist);
        }

        [HttpDelete("items/{wishlistItemId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveItem(
            Guid wishlistItemId)
        {
            await _wishlistService.RemoveItemAsync(
                GetUserId(),
                wishlistItemId);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.WishlistItemRemoved));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Clear()
        {
            await _wishlistService.ClearAsync(
                GetUserId());

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.WishlistCleared));
        }

        [HttpPost("move-to-cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MoveToCart()
        {
            await _wishlistService.MoveToCartAsync(
                GetUserId());

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.WishlistItemsMoved));
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("users/{userId:guid}")]
        [ProducesResponseType(
            typeof(WishlistResponseDto),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(
            Guid userId)
        {
            var wishlist =
                await _wishlistService
                    .GetWishlistByUserIdAsync(userId);

            if (wishlist == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.WishlistNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(wishlist);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("{wishlistId:guid}")]
        [ProducesResponseType(
            typeof(WishlistResponseDto),
            StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            Guid wishlistId)
        {
            var wishlist =
                await _wishlistService
                    .GetWishlistByIdAsync(wishlistId);

            if (wishlist == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.WishlistNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(wishlist);
        }
    }
}