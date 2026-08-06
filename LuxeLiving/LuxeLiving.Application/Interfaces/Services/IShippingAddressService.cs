using FurnitureShop.Application.DTOs.ShippingAddress;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IShippingAddressService
    {
        Task<ShippingAddressResponseDto> AddAsync(
            Guid userId,
            ShippingAddressRequestDto request);
        Task<List<ShippingAddressResponseDto>> GetMyAddressesAsync(Guid userId);
        Task<ShippingAddressResponseDto?> GetByIdAsync(
            Guid userId,
            Guid addressId);
        Task<ShippingAddressResponseDto?> GetDefaultAddressAsync(Guid userId);
        Task<ShippingAddressResponseDto> UpdateAsync(
            Guid userId,
            Guid addressId,
            ShippingAddressRequestDto request);
        Task SetDefaultAsync(Guid userId, Guid addressId);
        Task DeleteAsync(Guid userId, Guid addressId);
    }
}