using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IShippingAddressRepository
    {
        #region Create

        Task AddAsync(ShippingAddress address);

        #endregion

        #region Read

        Task<ShippingAddress?> GetByIdAsync(Guid addressId);

        Task<List<ShippingAddress>> GetByUserIdAsync(Guid userId);

        Task<ShippingAddress?> GetUserAddressAsync(
            Guid userId,
            Guid addressId);

        Task<ShippingAddress?> GetDefaultAsync(Guid userId);

        #endregion

        #region Validation

        Task<bool> ExistsAsync(
            Guid userId,
            Guid addressId);

        #endregion

        #region Update

        Task UpdateAsync(ShippingAddress address);

        #endregion

        #region Delete

        Task DeleteAsync(ShippingAddress address);

        #endregion

        #region Save

        Task SaveChangesAsync();

        #endregion
    }
}