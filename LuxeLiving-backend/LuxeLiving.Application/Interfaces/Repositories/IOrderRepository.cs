using LuxeLiving.Domain.Enitities;
using LuxeLiving.Domain.Enums;

namespace LuxeLiving.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<bool> ExistsOrderNumberAsync(string orderNumber);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<Order?> GetByIdAsync(Guid orderId, Guid userId);
        Task<List<Order>> GetByUserIdAsync(Guid userId);
        Task<List<Order>> GetAllAsync();
        Task UpdateAsync(Order order);
        Task<bool> HasPurchasedProductAsync(Guid userId, Guid orderId, Guid productId);
        Task<int> GetTotalProductsPurchasedAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task SaveChangesAsync();
    }
}