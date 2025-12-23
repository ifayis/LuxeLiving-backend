using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers;

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

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CheckoutRequestDto request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _orderService.CheckoutAsync(userId, request);
        return Ok("Order placed successfully");
    }
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var orders = await _orderService.GetMyOrdersAsync(userId);
        return Ok(orders);
    }

    [HttpGet("my/{orderId}")]
    public async Task<IActionResult> GetMyOrder(Guid orderId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var order = await _orderService.GetMyOrderByIdAsync(userId, orderId);

        if (order == null)
            return NotFound("Order not found");

        return Ok(order);
    }
}
