using FurnitureShop.Application.DTOs.User;
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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IShippingAddressRepository _shippingAddressRepository;


        public UserService(
            IUserRepository userRepository,
            ICartRepository cartRepository,
            IWishlistRepository wishlistRepository,
            IShippingAddressRepository shippingAddressRepository)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _wishlistRepository = wishlistRepository;
            _shippingAddressRepository = shippingAddressRepository;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                IsBlocked = u.IsBlocked
            }).ToList();
        }

        public async Task<SingleUserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            var cart = await _cartRepository.GetByUserIdAsync(user.Id);
            var wishlist = await _wishlistRepository.GetByUserIdAsync(user.Id);

            return new SingleUserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CartId = cart?.Id,
                WishlistId = wishlist?.Id,
                IsBlocked = user.IsBlocked,

                ShippingAddress = user.ShippingAddress == null
            ? null
            : new ShippingAddressResponseDto
            {
                Id = user.ShippingAddress.Id,
                FullName = user.ShippingAddress.FullName,
                PhoneNumber = user.ShippingAddress.PhoneNumber,
                AddressLine1 = user.ShippingAddress.AddressLine1,
                AddressLine2 = user.ShippingAddress.AddressLine2,
                City = user.ShippingAddress.City,
                PinCode = user.ShippingAddress.PinCode
            }
            };
        }

        public async Task<bool> BlockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsBlocked = true;
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnblockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsBlocked = false;
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task AddOrUpdateShippingAddressAsync(Guid userId, AddShippingAddressRequestDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new Exception("User not found");

            var address = await _shippingAddressRepository.GetByUserIdAsync(userId);

            if (address == null)
            {
                address = new ShippingAddress
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _shippingAddressRepository.AddAsync(address);
                user.ShippingAddressId = address.Id;
            }

            address.FullName = dto.FullName;
            address.PhoneNumber = dto.PhoneNumber;
            address.AddressLine1 = dto.AddressLine1;
            address.AddressLine2 = dto.AddressLine2;
            address.City = dto.City;
            address.PinCode = dto.PinCode;

            await _userRepository.SaveChangesAsync();
        }


    }
}