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
            var address = new ShippingAddress
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                PinCode = dto.PinCode,
                CreatedAt = DateTime.UtcNow
            };

            await _shippingaddressrepository.AddAsync(address);
        }

        public async Task<List<ShippingAddressResponseDto>> GetMyAsync(Guid userId)
        {
            var addresses = await _shippingaddressrepository.GetByUserIdAsync(userId);

            return addresses.Select(a => new ShippingAddressResponseDto
            {
                Id = a.Id,
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                PinCode = a.PinCode
            }).ToList();
        }

        public async Task UpdateAsync(Guid userId, Guid addressId, ShippingAddressRequestDto dto)
        {
            var address = await _shippingaddressrepository.GetByIdAsync(addressId)
                ?? throw new Exception("Address not found");

            if (address.UserId != userId)
                throw new UnauthorizedAccessException();

            address.FullName = dto.FullName;
            address.PhoneNumber = dto.PhoneNumber;
            address.AddressLine1 = dto.AddressLine1;
            address.AddressLine2 = dto.AddressLine2;
            address.City = dto.City;
            address.PinCode = dto.PinCode;
            address.UpdatedAt = DateTime.UtcNow;

            await _shippingaddressrepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId, Guid addressId)
        {
            var address = await _shippingaddressrepository.GetByIdAsync(addressId)
                ?? throw new Exception("Address not found");

            if (address.UserId != userId)
                throw new UnauthorizedAccessException();

            await _shippingaddressrepository.DeleteAsync(address);
        }
    }
}
