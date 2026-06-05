using FurnitureShop.Application.DTOs.ShippingAddress;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Services
{
    public class ShippingAddressService : IShippingAddressService
    {
        private readonly IShippingAddressRepository _shippingaddressrepository;

        public ShippingAddressService(IShippingAddressRepository repository)
        {
            _shippingaddressrepository = repository;
        }

        public async Task AddAsync(Guid userId, ShippingAddressRequestDto dto)
        {
            var existingAddresses =
            await _shippingaddressrepository.GetByUserIdAsync(userId);

            if (existingAddresses.Count >= 5)
            {
                throw new InvalidOperationException(
                    "Maximum 5 shipping addresses allowed.");
            }

            var address = new ShippingAddress
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FullName = dto.FullName.Trim(),
                PhoneNumber = dto.PhoneNumber.Trim(),
                AddressLine1 = dto.AddressLine1.Trim(),
                AddressLine2 = dto.AddressLine2?.Trim(),
                City = dto.City.Trim(),
                PinCode = dto.PinCode.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _shippingaddressrepository.AddAsync(address);
        }

        public async Task<List<ShippingAddressResponseDto>> GetMyAsync(Guid userId)
        {
            var existingAddresses =
            await _shippingaddressrepository.GetByUserIdAsync(userId);

            var isFirstAddress = !existingAddresses.Any();
            var addresses = await _shippingaddressrepository.GetByUserIdAsync(userId);

            return addresses.Select(a => new ShippingAddressResponseDto
            {
                Id = a.Id,
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                PinCode = a.PinCode,
                IsDefault = a.IsDefault
            }).ToList();
        }

        public async Task UpdateAsync(Guid userId, Guid addressId, ShippingAddressRequestDto dto)
        {
            var address = await _shippingaddressrepository.GetByIdAsync(addressId)
                ?? throw new KeyNotFoundException("Address not found");

            if (address.UserId != userId)
                throw new UnauthorizedAccessException();

            address.FullName = dto.FullName.Trim();
            address.PhoneNumber = dto.PhoneNumber.Trim();
            address.AddressLine1 = dto.AddressLine1.Trim();
            address.AddressLine2 = dto.AddressLine2?.Trim();
            address.City = dto.City.Trim();
            address.PinCode = dto.PinCode.Trim();
            address.UpdatedAt = DateTime.UtcNow;

            await _shippingaddressrepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId, Guid addressId)
        {
            var address = await _shippingaddressrepository.GetByIdAsync(addressId)
                ?? throw new KeyNotFoundException("Address not found");

            if (address.UserId != userId)
                throw new UnauthorizedAccessException();

            await _shippingaddressrepository.DeleteAsync(address);
        }

        public async Task SetDefaultAsync(
            Guid userId,
            Guid addressId)
        {
            var addresses =
                await _shippingaddressrepository
                    .GetByUserIdAsync(userId);

            var selectedAddress =
                addresses.FirstOrDefault(a => a.Id == addressId);

            if (selectedAddress == null)
            {
                throw new KeyNotFoundException(
                    "Address not found");
            }

            foreach (var address in addresses)
            {
                address.IsDefault = false;
            }

            selectedAddress.IsDefault = true;

            await _shippingaddressrepository.SaveChangesAsync();
        }
    }
}
