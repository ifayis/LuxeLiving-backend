using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task CheckoutAsync(Guid userId, CheckoutRequestDto request);
        Task<List<OrderResponseDto>> GetMyOrdersAsync(Guid userId);
        Task<OrderResponseDto?> GetMyOrderByIdAsync(Guid userId, Guid orderId);
        Task<OrderResponseDto?> CancelOrderAsync(Guid userId, Guid orderId);

    }
}
