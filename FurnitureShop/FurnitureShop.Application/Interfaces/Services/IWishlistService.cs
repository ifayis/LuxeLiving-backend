using FurnitureShop.Application.DTOs.Wishlist;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IWishlistService
    {
        Task<WishlistResponseDto> AddAsync(Guid userId, AddToWishlistRequestDto request);
        Task<WishlistResponseDto?> GetMyAsync(Guid userId);
        Task MoveToCartAsync(Guid userId);
        Task RemoveItemAsync(Guid userId, Guid itemId);
        Task ClearAsync(Guid userId);
    }
}
