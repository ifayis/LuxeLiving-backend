using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Application.Interfaces.Services;
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
    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("Add")]
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

    [HttpPut("cancel/{orderId}")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var order = await _orderService.CancelOrderAsync(userId, orderId);

        if (order == null)
            return NotFound("Order not found");

        return Ok(order);
    }


}
