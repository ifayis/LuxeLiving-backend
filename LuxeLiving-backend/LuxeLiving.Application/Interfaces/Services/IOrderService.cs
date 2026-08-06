using LuxeLiving.Application.DTOs.Order;
using LuxeLiving.Domain.Enums;

namespace LuxeLiving.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetMyOrdersAsync( Guid userId);
        Task<OrderResponseDto?> GetMyOrderAsync(Guid userId, Guid orderId);
        Task<List<OrderResponseDto>> GetAllOrdersAsync();
        Task<List<OrderResponseDto>> GetOrdersByUserAsync(Guid userId);
        Task<OrderResponseDto?> GetOrderAsync(Guid orderId);
        Task<OrderResponseDto> UpdateStatusAsync(
            Guid orderId,
            UpdateOrderStatusRequestDto request);
        Task<OrderResponseDto?> CancelOrderAsync(
            Guid userId,
            Guid orderId,
            CancelOrderRequestDto request);
        Task<int> GetTotalProductsPurchasedAsync();
        Task<decimal> GetTotalRevenueAsync();
    }
}