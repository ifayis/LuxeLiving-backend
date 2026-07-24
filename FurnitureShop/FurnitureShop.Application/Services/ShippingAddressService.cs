using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.ShippingAddress;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public partial class ShippingAddressService
        : IShippingAddressService
    {
        private readonly IShippingAddressRepository
            _shippingAddressRepository;

        public ShippingAddressService(
            IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepository =
                shippingAddressRepository;
        }

        public async Task<ShippingAddressResponseDto> AddAsync(
            Guid userId,
            ShippingAddressRequestDto request)
        {
            var existingAddresses =
                await _shippingAddressRepository
                    .GetByUserIdAsync(userId);

            if (existingAddresses.Count >= 5)
            {
                throw new InvalidOperationException(
                    ErrorMessages.MaximumShippingAddressesReached);
            }

            var address = new ShippingAddress
            {
                Id = Guid.NewGuid(),

                UserId = userId,

                FullName = request.FullName.Trim(),

                PhoneNumber = request.PhoneNumber.Trim(),

                AddressLine1 = request.AddressLine1.Trim(),

                AddressLine2 = request.AddressLine2?.Trim(),

                City = request.City.Trim(),

                State = request.State.Trim(),

                Country = request.Country.Trim(),

                PinCode = request.PinCode.Trim(),

                AddressType = request.AddressType,

                IsDefault = !existingAddresses.Any(),

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            await _shippingAddressRepository
                .AddAsync(address);

            await _shippingAddressRepository
                .SaveChangesAsync();

            return Map(address);
        }

        public async Task<List<ShippingAddressResponseDto>>GetMyAddressesAsync(Guid userId)
        {
            var addresses =
                await _shippingAddressRepository
                    .GetByUserIdAsync(userId);

            return addresses
                .Select(Map)
                .ToList();
        }

        public async Task<ShippingAddressResponseDto?>GetByIdAsync(
                Guid userId,
                Guid addressId)
        {
            var address =
                await _shippingAddressRepository
                    .GetUserAddressAsync(
                        userId,
                        addressId);

            if (address == null)
                return null;

            return Map(address);
        }

        public async Task<ShippingAddressResponseDto?>GetDefaultAddressAsync(Guid userId)
        {
            var address =
                await _shippingAddressRepository
                    .GetDefaultAsync(userId);

            if (address == null)
                return null;

            return Map(address);
        }

        public async Task<ShippingAddressResponseDto> UpdateAsync(
            Guid userId,
            Guid addressId,
            ShippingAddressRequestDto request)
        {
            var address = await _shippingAddressRepository
                .GetUserAddressAsync(userId, addressId);

            if (address == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.AddressNotFound);
            }

            address.FullName = request.FullName.Trim();

            address.PhoneNumber = request.PhoneNumber.Trim();

            address.AddressLine1 = request.AddressLine1.Trim();

            address.AddressLine2 = request.AddressLine2?.Trim();

            address.City = request.City.Trim();

            address.State = request.State.Trim();

            address.Country = request.Country.Trim();

            address.PinCode = request.PinCode.Trim();

            address.AddressType = request.AddressType;

            address.UpdatedAt = DateTime.UtcNow;

            await _shippingAddressRepository
                .UpdateAsync(address);

            await _shippingAddressRepository
                .SaveChangesAsync();

            return Map(address);
        }
        public async Task DeleteAsync(
            Guid userId,
            Guid addressId)
        {
            var address = await _shippingAddressRepository
                .GetUserAddressAsync(userId, addressId);

            if (address == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.AddressNotFound);
            }

            var wasDefault = address.IsDefault;

            await _shippingAddressRepository
                .DeleteAsync(address);

            if (wasDefault)
            {
                var remainingAddresses =
                    await _shippingAddressRepository
                        .GetByUserIdAsync(userId);

                var newDefault = remainingAddresses
                    .FirstOrDefault(x => x.Id != addressId);

                if (newDefault != null)
                {
                    newDefault.IsDefault = true;

                    await _shippingAddressRepository
                        .UpdateAsync(newDefault);
                }
            }

            await _shippingAddressRepository
                .SaveChangesAsync();
        }

        public async Task SetDefaultAsync(
            Guid userId,
            Guid addressId)
        {
            var addresses =
                await _shippingAddressRepository
                    .GetByUserIdAsync(userId);

            var selectedAddress =
                addresses.FirstOrDefault(x =>
                    x.Id == addressId);

            if (selectedAddress == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.AddressNotFound);
            }

            foreach (var address in addresses)
            {
                address.IsDefault = false;

                await _shippingAddressRepository
                    .UpdateAsync(address);
            }

            selectedAddress.IsDefault = true;

            await _shippingAddressRepository
                .UpdateAsync(selectedAddress);

            await _shippingAddressRepository
                .SaveChangesAsync();
        }

        private static ShippingAddressResponseDto Map(
            ShippingAddress address)
        {
            return new ShippingAddressResponseDto
            {
                Id = address.Id,

                FullName = address.FullName,

                PhoneNumber = address.PhoneNumber,

                AddressLine1 = address.AddressLine1,

                AddressLine2 = address.AddressLine2,

                City = address.City,

                State = address.State,

                Country = address.Country,

                PinCode = address.PinCode,

                AddressType = address.AddressType,

                IsDefault = address.IsDefault,

                CreatedAt = address.CreatedAt,

                UpdatedAt = address.UpdatedAt
            };
        }
    }
}
