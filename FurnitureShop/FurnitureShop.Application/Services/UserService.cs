using FurnitureShop.Application.DTOs.User;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
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

        public UserService(
            IUserRepository userRepository,
            ICartRepository cartRepository,
            IWishlistRepository wishlistRepository)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _wishlistRepository = wishlistRepository;
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

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            var cart = await _cartRepository.GetByUserIdAsync(user.Id);
            var wishlist = await _wishlistRepository.GetByUserIdAsync(user.Id);

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CartId = cart?.Id,
                WishlistId = wishlist?.Id,
                IsBlocked = user.IsBlocked
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

    }
}