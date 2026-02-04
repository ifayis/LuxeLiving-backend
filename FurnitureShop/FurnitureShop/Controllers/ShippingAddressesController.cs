using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.ShippingAddress;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/shipping-addresses")]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly IShippingAddressService _shippingaddressService;

        public ShippingAddressesController(IShippingAddressService service)
        {
            _shippingaddressService = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(ShippingAddressRequestDto dto)
        {
            await _shippingaddressService.AddAsync(GetUserId(), dto);
            return Ok(
                ApiResponse<object>.Success(
                null,
                ResponseMessages.AddressAdded
                ));
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            return Ok(await _shippingaddressService.GetMyAsync(GetUserId()));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            return Ok(await _shippingaddressService.GetMyAsync(userId));
        }

        [Authorize]
        [HttpPut("{addressId}")]
        public async Task<IActionResult> Update(Guid addressId, ShippingAddressRequestDto dto)
        {
            await _shippingaddressService.UpdateAsync(GetUserId(), addressId, dto);
            return Ok(
                 ApiResponse<object>.Success(
                 null,
                 ResponseMessages.AddressUpdated
                 ));
        }

        [Authorize]
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> Delete(Guid addressId)
        {
            await _shippingaddressService.DeleteAsync(GetUserId(), addressId);
            return Ok(
                 ApiResponse<object>.Success(
                 null,
                 ResponseMessages.AddressDeleted
                 ));
        }

        private Guid GetUserId()
            => Guid.Parse(User.FindFirst("UID")!.Value);
    }
}
