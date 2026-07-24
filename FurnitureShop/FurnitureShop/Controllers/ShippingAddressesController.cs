using FurnitureShop.API.Common;
using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.ShippingAddress;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/shipping-addresses")]
    [Authorize(Roles = Roles.User)]
    [Produces("application/json")]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly IShippingAddressService
            _shippingAddressService;

        public ShippingAddressesController(
            IShippingAddressService shippingAddressService)
        {
            _shippingAddressService =
                shippingAddressService;
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
            typeof(ShippingAddressResponseDto),
            StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] ShippingAddressRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            var address =
                await _shippingAddressService
                    .AddAsync(GetUserId(), request);

            return CreatedAtAction(
                nameof(GetById),
                new { addressId = address.Id },
                address);
        }

        [HttpGet]
        [ProducesResponseType(
            typeof(List<ShippingAddressResponseDto>),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyAddresses()
        {
            return Ok(
                await _shippingAddressService
                    .GetMyAddressesAsync(GetUserId()));
        }

        [HttpGet("{addressId:guid}")]
        [ProducesResponseType(
            typeof(ShippingAddressResponseDto),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            Guid addressId)
        {
            var address =
                await _shippingAddressService
                    .GetByIdAsync(
                        GetUserId(),
                        addressId);

            if (address == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.AddressNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(address);
        }

        [HttpGet("default")]
        [ProducesResponseType(
            typeof(ShippingAddressResponseDto),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDefault()
        {
            var address =
                await _shippingAddressService
                    .GetDefaultAddressAsync(
                        GetUserId());

            if (address == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.AddressNotFound,
                        StatusCodes.Status404NotFound));
            }

            return Ok(address);
        }

        [HttpPut("{addressId:guid}")]
        [ProducesResponseType(
            typeof(ShippingAddressResponseDto),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(
            Guid addressId,
            [FromBody] ShippingAddressRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            var address =
                await _shippingAddressService
                    .UpdateAsync(
                        GetUserId(),
                        addressId,
                        request);

            return Ok(address);
        }

        [HttpPatch("{addressId:guid}/default")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        public async Task<IActionResult> SetDefault(
            Guid addressId)
        {
            await _shippingAddressService
                .SetDefaultAsync(
                    GetUserId(),
                    addressId);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.DefaultAddressUpdated));
        }

        [HttpDelete("{addressId:guid}")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(
            Guid addressId)
        {
            await _shippingAddressService
                .DeleteAsync(
                    GetUserId(),
                    addressId);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    ResponseMessages.AddressDeleted));
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("users/{userId:guid}")]
        public async Task<IActionResult> GetUserAddresses(
            Guid userId)
        {
            return Ok(
                await _shippingAddressService
                    .GetMyAddressesAsync(userId));
        }
    }
}