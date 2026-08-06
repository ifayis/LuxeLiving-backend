using LuxeLiving.Domain.Entities;

namespace LuxeLiving.Application.Interfaces.Repositories
{
    public interface IShippingAddressRepository
    {
        Task AddAsync(ShippingAddress address);
        Task<ShippingAddress?> GetByIdAsync(Guid addressId);
        Task<List<ShippingAddress>> GetByUserIdAsync(Guid userId);
        Task<ShippingAddress?> GetUserAddressAsync(Guid userId, Guid addressId);
        Task<ShippingAddress?> GetDefaultAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId, Guid addressId);
        Task UpdateAsync(ShippingAddress address);
        Task DeleteAsync(ShippingAddress address);
        Task SaveChangesAsync();
    }
}