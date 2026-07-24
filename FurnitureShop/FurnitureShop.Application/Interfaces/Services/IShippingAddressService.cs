using FurnitureShop.Application.DTOs.ShippingAddress;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IShippingAddressService
    {
        #region Create

        Task<ShippingAddressResponseDto> AddAsync(
            Guid userId,
            ShippingAddressRequestDto request);

        #endregion

        #region Read

        Task<List<ShippingAddressResponseDto>> GetMyAddressesAsync(
            Guid userId);

        Task<ShippingAddressResponseDto?> GetByIdAsync(
            Guid userId,
            Guid addressId);

        Task<ShippingAddressResponseDto?> GetDefaultAddressAsync(
            Guid userId);

        #endregion

        #region Update

        Task<ShippingAddressResponseDto> UpdateAsync(
            Guid userId,
            Guid addressId,
            ShippingAddressRequestDto request);

        Task SetDefaultAsync(
            Guid userId,
            Guid addressId);

        #endregion

        #region Delete

        Task DeleteAsync(
            Guid userId,
            Guid addressId);

        #endregion
    }
}