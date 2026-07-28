using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IWishlistRepository
    {
        Task AddAsync(Wishlist wishlist);
        Task AddWishlistItemAsync(WishlistItem wishlistItem);
        Task<Wishlist?> GetByIdAsync(Guid wishlistId);
        Task<Wishlist?> GetByUserIdAsync(Guid userId);
        Task<WishlistItem?> GetWishlistItemAsync(Guid wishlistId, Guid productId);
        Task<WishlistItem?> GetWishlistItemByIdAsync(Guid wishlistItemId);
        Task<bool> ExistsAsync(Guid wishlistId, Guid productId);
        Task UpdateAsync(Wishlist wishlist);
        Task RemoveWishlistItemAsync(WishlistItem wishlistItem);
        Task ClearWishlistAsync(Wishlist wishlist);
        Task SaveChangesAsync();
    }
}