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

        public OrdersController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }


        private Guid GetUserId()
        {
            var userId = User.FindFirstValue("UID");

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "User not found.");
            }

            return Guid.Parse(userId);
        }


        [Authorize(Roles = Roles.User)]
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService
                .GetMyOrdersAsync(GetUserId());

            return Ok(orders);
        }


        [Authorize(Roles = Roles.User)]
        [HttpGet("my-orders/{orderId:guid}")]
        public async Task<IActionResult> GetMyOrder(
            Guid orderId)
        {
            var order = await _orderService
                .GetMyOrderAsync(
                    GetUserId(),
                    orderId
                );

            if (order == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.OrderNotFound,
                        404
                    )
                );
            }

            return Ok(order);
        }


        [Authorize(Roles = Roles.User)]
        [HttpPatch("{orderId:guid}/cancel")]
        public async Task<IActionResult> CancelOrder(
            Guid orderId,
            CancelOrderRequestDto request)
        {
            var order = await _orderService
                .CancelOrderAsync(
                    GetUserId(),
                    orderId,
                    request
                );

            if (order == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.OrderNotFound,
                        404
                    )
                );
            }

            return Ok(ApiResponse<OrderResponseDto>.Success(
                order,
                "Order cancelled successfully.")
            );
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService
                .GetAllOrdersAsync();

            return Ok(orders);
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("users/{userId:guid}")]
        public async Task<IActionResult> GetOrdersByUser(
            Guid userId)
        {
            var orders = await _orderService
                .GetOrdersByUserAsync(userId);

            return Ok(orders);
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrder(
            Guid orderId)
        {
            var order = await _orderService
                .GetOrderAsync(orderId);

            if (order == null)
            {
                return NotFound(
                    ApiResponse<object>.Fail(
                        ErrorMessages.OrderNotFound,
                        404
                    )
                );
            }

            return Ok(order);
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("{orderId:guid}/status")]
        public async Task<IActionResult> UpdateStatus(
            Guid orderId,
            UpdateOrderStatusRequestDto request)
        {
            var order = await _orderService
                .UpdateStatusAsync(
                    orderId,
                    request
                );

            return Ok(ApiResponse<OrderResponseDto>.Success(
                order,
                "Order status updated successfully.")
            );
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("dashboard/total-products")]
        public async Task<IActionResult> TotalProductsPurchased()
        {
            var total = await _orderService
                .GetTotalProductsPurchasedAsync();

            return Ok(new ProductSalesSummaryDto
            {
                TotalProductsPurchased = total
            });
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("dashboard/total-revenue")]
        public async Task<IActionResult> TotalRevenue()
        {
            var revenue = await _orderService
                .GetTotalRevenueAsync();

            return Ok(new RevenueSummaryDto
            {
                TotalRevenue = revenue
            });
        }
    }
}