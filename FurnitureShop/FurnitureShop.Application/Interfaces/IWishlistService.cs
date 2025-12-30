using FurnitureShop.Application.DTOs.Wishlist;

namespace FurnitureShop.Application.Interfaces
{
    public interface IWishlistService
    {
        Task AddAsync(Guid userId, AddToWishlistRequestDto request);
        Task<WishlistResponseDto?> GetMyWishlistAsync(Guid userId);
        Task<WishlistResponseDto?> GetByIdAsync(Guid id);
        Task RemoveItemAsync(Guid userId, Guid itemId);
        Task ClearAsync(Guid userId);
    }
}
