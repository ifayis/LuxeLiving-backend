using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
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
                throw new UnauthorizedAccessException();

            return Guid.Parse(userId);
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("my-orders")]
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

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("Individual:    {userId:guid}")]
        public async Task<IActionResult> GetOrdersByUser(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId);

            if (orders == null || !orders.Any())
                return NotFound();

            return Ok(orders);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("Total-Products")]
        public async Task<IActionResult> TotalProductsPurchased()
        {
            var total = await _orderService.GetTotalProductsPurchasedAsync();
            return Ok(total);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("Total-Revenue")]
        public async Task<IActionResult> TotalRevenue()
        {
            var revenue = await _orderService.GetTotalRevenueAsync();
            return Ok(revenue);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("Order-Details:{orderId}")]
        public async Task<IActionResult> OrderDetails(Guid orderId)
        {
            var order = await _orderService.GetOrderDetailsAsync(orderId);
            if (order == null) return NotFound("Order not found");

            return Ok(order);
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut("cancel:{orderId:guid}")]
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
