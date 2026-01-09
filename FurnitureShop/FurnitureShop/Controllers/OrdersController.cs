using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
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
        public async Task<IActionResult> Checkout(CheckoutRequestDto request)
        {
            await _orderService.CheckoutAsync(GetUserId(), request);

            return Ok();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrdersAsync(GetUserId());

            if (orders == null || !orders.Any())
            {
                return Ok(
                           ApiResponse<object>.Success(
                               Array.Empty<object>(),
                               ResponseMessages.empty
                           )
                       );
            }

            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetMyOrder(Guid orderId)
        {
            var order = await _orderService.GetMyOrderByIdAsync(GetUserId(), orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPut("cancel/{orderId:guid}")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var order = await _orderService.CancelOrderAsync(GetUserId(), orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
