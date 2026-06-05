using FurnitureShop.Application.common;
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
        private readonly IProductRepository _productRepository;

        public OrderService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<List<OrderResponseDto>> GetMyOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(Map).ToList();
        }

        public async Task<OrderResponseDto?> GetMyOrderByIdAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, userId);
            return order == null ? null : Map(order);
        }

        public async Task<List<OrderResponseDto>> GetOrdersByUserAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return orders.Select(o => Map(o)).ToList();
        }

        public async Task<OrderResponseDto?> CancelOrderAsync(Guid userId, Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, userId);
            if (order == null) return null;

            if (order.Status == "Paid")
            {
                order.Status = OrderStatuses.Cancelled;
                order.PaymentMethod = PaymentMethods.Refunded;
            }
            else if (order.Status == OrderStatuses.Pending)
            {
                order.Status = OrderStatuses.Cancelled;
                order.PaymentMethod = PaymentMethods.CodCancelled;
            }

            if (order.Status == OrderStatuses.Delivered)
            {
                throw new InvalidOperationException(
                    "Delivered orders cannot be cancelled");
            }

            if (order.Status == OrderStatuses.Cancelled)
            {
                throw new InvalidOperationException(
                    "Order already cancelled");
            }

            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product == null)
                    continue;

                product.StockQuantity += item.Quantity;
            }

            await _productRepository.SaveChangesAsync();

            await _orderRepository.UpdateAsync(order);

            return Map(order);
        }

        public async Task<int> GetTotalProductsPurchasedAsync()
        {
            return await _orderRepository.GetTotalProductsPurchasedAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _orderRepository.GetTotalRevenueAsync();
        }

        public async Task<OrderResponseDto?> GetOrderDetailsAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderDetailsAsync(orderId);
            if (order == null) return null;

            return Map(order);
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
                    ProductName = i.Product.Name ?? string.Empty,
                    ImageUrl = i.Product?.ImageUrl,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

    }
}
