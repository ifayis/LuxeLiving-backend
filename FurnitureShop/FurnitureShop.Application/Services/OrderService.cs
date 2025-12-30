using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        public async Task CheckoutAsync(Guid userId, CheckoutRequestDto request)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
                throw new Exception("Cart is empty");

            var total = cart.Items.Sum(i => i.Quantity * 100); // dummy price logic

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TotalAmount = total,
                Status = request.SimulatePaymentSuccess ? "Paid" : "Failed",
                Items = cart.Items.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = 100 // dummy price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            if (order.Status == "Paid")
                await _cartRepository.ClearCartAsync(cart.Id);
        }
        public async Task<List<OrderResponseDto>> GetMyOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return orders.Select(o => new OrderResponseDto
            {
                OrderId = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            }).ToList();
        }

        public async Task<OrderResponseDto?> GetMyOrderByIdAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, userId);
            if (order == null) return null;

            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }
    }
}
