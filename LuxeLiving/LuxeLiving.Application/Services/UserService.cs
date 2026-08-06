using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Common;
using FurnitureShop.Application.DTOs.User;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;

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

        public async Task<PagedResponseDto<UserResponseDto>>GetAllUsersAsync(
                int pageNumber,
                int pageSize)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var totalRecords =
                await _userRepository.CountAsync();

            var users =
                await _userRepository.GetPagedAsync(
                    pageNumber,
                    pageSize
                );

            return new PagedResponseDto<UserResponseDto>
            {
                Items = users
                    .Select(u => new UserResponseDto
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        Role = u.Role,
                        IsBlocked = u.IsBlocked
                    })
                    .ToList(),

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,

                TotalPages =
                    (int)Math.Ceiling(
                        totalRecords /
                        (double)pageSize
                    )
            };
        }

        public async Task<SingleUserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.UserNotFound);
            }

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
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsEmailVerified = user.IsEmailVerified
            };
        }

        public async Task BlockUserAsync(Guid userId, Guid currentAdminId)
        {

            if (userId == currentAdminId)
            {
                throw new InvalidOperationException(
                    "You cannot block your own account.");
            }

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.UserNotFound);
            }

            if (user.Role == Roles.Admin)
            {
                throw new InvalidOperationException(
                    "Admin accounts cannot be blocked.");
            }

            user.IsBlocked = true;

            await _userRepository.SaveChangesAsync();

        }

        public async Task UnblockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.UserNotFound);
            }

            user.IsBlocked = false;
            await _userRepository.SaveChangesAsync();
        }
    }
}