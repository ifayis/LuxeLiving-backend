using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Checkout;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    [Authorize(Roles = Roles.User)]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        private Guid GetUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(id))
                throw new UnauthorizedAccessException("User not found");

            return Guid.Parse(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckout()
        {
            var result = await _checkoutService.GetCheckoutAsync(GetUserId());
            return Ok(result);
        }

        [HttpPost("payment")]
        public async Task<IActionResult> Pay(PaymentRequestDto request)
        {
            await _checkoutService.ExecutePaymentAsync(GetUserId(), request);

            return Ok(ApiResponse<object>.Success(
                null,
                "Payment successful. Order created."
            ));
        }
    }
}
