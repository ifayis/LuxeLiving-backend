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
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

    [HttpPost("add")]
        public async Task<IActionResult> Add(AddToWishlistRequestDto request)
        {
            try
            {
              await _wishlistService.AddAsync(GetUserId(), request);

                return Ok(
                    ApiResponse<object>.Success(
                    null,
                    ResponseMessages.WishlistItemAdded
                    )
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                    ex.Message,
                    400
                    )
                );
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(
                    ApiResponse<object>.Fail(
                    ex.Message,
                    409
                    )
                );
            }
        }

        [HttpGet("my-Wishlist")]
        public async Task<IActionResult> GetMy()
        {
            var wishlist = await _wishlistService.GetMyWishlistAsync(GetUserId());

            return Ok(
                ApiResponse<object>.Success(
                    wishlist,
                    ResponseMessages.WishlistFetched
                )
            );
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _wishlistService.GetByIdAsync(id);

            if (item == null)
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.NotFound,
                        404
                    )
                );

            return Ok(
                ApiResponse<object>.Success(
                    item,
                    ResponseMessages.Success
                )
            );
        }

        [HttpPatch("item/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            try
            {
                await _wishlistService.RemoveItemAsync(GetUserId(), itemId);

                return Ok(
                    ApiResponse<object>.Success(
                        null,
                        ResponseMessages.WishlistItemRemoved
                    )
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ex.Message,
                        400
                    )
                );
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ex.Message,
                        404
                    )
                );
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            try
            {
                await _wishlistService.ClearAsync(GetUserId());

                return Ok(
                    ApiResponse<object>.Success(
                        null,
                        ResponseMessages.WishlistCleared
                    )
                );
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ex.Message,
                        404
                    )
                );
            }
        }
    }
}
