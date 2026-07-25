using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IOrderService
    {
        #region Customer

        Task<List<OrderResponseDto>> GetMyOrdersAsync(
            Guid userId);

        Task<OrderResponseDto?> GetMyOrderAsync(
            Guid userId,
            Guid orderId);

        #endregion

        #region Admin

        Task<List<OrderResponseDto>> GetAllOrdersAsync();

        Task<List<OrderResponseDto>> GetOrdersByUserAsync(
            Guid userId);

        Task<OrderResponseDto?> GetOrderAsync(
            Guid orderId);

        Task<OrderResponseDto> UpdateStatusAsync(
            Guid orderId,
            UpdateOrderStatusRequestDto request);

        Task<OrderResponseDto?> CancelOrderAsync(
            Guid userId,
            Guid orderId,
            CancelOrderRequestDto request);

        #endregion

        #region Dashboard

        Task<int> GetTotalProductsPurchasedAsync();

        Task<decimal> GetTotalRevenueAsync();

        #endregion
    }
}