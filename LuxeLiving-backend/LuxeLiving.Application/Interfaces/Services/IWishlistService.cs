using LuxeLiving.Application.DTOs.Wishlist;

namespace LuxeLiving.Application.Interfaces.Services
{
    public interface IWishlistService
    {
        Task<WishlistResponseDto> AddAsync(
            Guid userId,
            AddToWishlistRequestDto request);
        Task<WishlistResponseDto?> GetMyWishlistAsync(Guid userId);
        Task<WishlistResponseDto?> GetWishlistByUserIdAsync(Guid userId);
        Task<WishlistResponseDto?> GetWishlistByIdAsync(Guid wishlistId);
        Task MoveToCartAsync(Guid userId);
        Task RemoveItemAsync(Guid userId, Guid wishlistItemId);
        Task ClearAsync(Guid userId);
    }
}