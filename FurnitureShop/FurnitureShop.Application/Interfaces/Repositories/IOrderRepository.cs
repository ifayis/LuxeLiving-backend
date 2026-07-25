using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        #region Create

        Task AddAsync(Order order);

        #endregion

        #region Read

        Task<Order?> GetByIdAsync(Guid orderId);

        Task<bool> ExistsOrderNumberAsync(string orderNumber);

        Task<Order?> GetByOrderNumberAsync(
            string orderNumber);

        Task<Order?> GetByIdAsync(
            Guid orderId,
            Guid userId);

        Task<List<Order>> GetByUserIdAsync(
            Guid userId);

        Task<List<Order>> GetAllAsync();

        #endregion

        #region Update

        Task UpdateAsync(Order order);

        #endregion

        #region Analytics

        Task<int> GetTotalProductsPurchasedAsync();

        Task<decimal> GetTotalRevenueAsync();

        #endregion

        #region Persistence

        Task SaveChangesAsync();

        #endregion
    }
}