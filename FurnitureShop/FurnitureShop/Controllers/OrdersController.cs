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
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

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

            return Ok(orders);
        }

        [HttpGet("my/{orderId:guid}")]
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
