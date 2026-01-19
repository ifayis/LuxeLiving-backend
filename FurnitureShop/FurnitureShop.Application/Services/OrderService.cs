using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponseDto>> GetMyOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return orders.Select(o => new OrderResponseDto
            {
                OrderId = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                PaymentMethod = o.PaymentMethod,
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
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };
        }

        public async Task<List<OrderResponseDto>> GetOrdersByUserAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return orders.Select(o => Map(o)).ToList();
        }


        public async Task<OrderResponseDto?> CancelOrderAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, userId);

            if (order == null)
                return null;

            if (order.Status == "Cancelled")
                throw new Exception("Order already cancelled");

            if (order.Status == "Paid")
            {
                order.Status = "Cancelled";
                order.PaymentMethod = "Will be Refunded";
            }
            else if (order.Status == "Pending")
            {
                order.Status = "Cancelled";
                order.PaymentMethod = "COD - Cancelled";
            }

            await _orderRepository.UpdateAsync(order);

            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        private static OrderResponseDto Map(Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                }).ToList()
            };
        }

    }
}
