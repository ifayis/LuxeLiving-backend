using LuxeLiving.Application.common;
using LuxeLiving.Application.Common;
using LuxeLiving.Application.DTOs.Checkout;
using LuxeLiving.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LuxeLiving.API.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    [Authorize(Roles = Roles.User)]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(
            ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }


        private Guid GetUserId()
        {
            var id = User.FindFirstValue("UID");

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new UnauthorizedAccessException(
                    "User not found.");
            }

            return Guid.Parse(id);
        }


        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _checkoutService
                .GetSummaryAsync(GetUserId());

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(
            CheckoutRequestDto request)
        {
            var result = await _checkoutService
                .CheckoutAsync(
                    GetUserId(),
                    request
                );

            return Ok(ApiResponse<PaymentResponseDto>.Success(
                result,
                ResponseMessages.ExecutePayment)
            );
        }
    }
}