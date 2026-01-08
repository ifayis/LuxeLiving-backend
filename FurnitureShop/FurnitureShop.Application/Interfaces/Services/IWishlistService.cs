using FurnitureShop.Application.DTOs.Wishlist;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IWishlistService
    {
        Task AddAsync(Guid userId, AddToWishlistRequestDto request);
        Task<WishlistResponseDto?> GetMyAsync(Guid userId);
        Task MoveToCartAsync(Guid userId, Guid wishlistItemId);

        Task RemoveItemAsync(Guid userId, Guid itemId);
        Task ClearAsync(Guid userId);
    }
}
