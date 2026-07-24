using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IWishlistRepository
    {
        #region Create

        Task AddAsync(Wishlist wishlist);

        Task AddWishlistItemAsync(WishlistItem wishlistItem);

        #endregion

        #region Read

        Task<Wishlist?> GetByIdAsync(Guid wishlistId);

        Task<Wishlist?> GetByUserIdAsync(Guid userId);

        Task<WishlistItem?> GetWishlistItemAsync(
            Guid wishlistId,
            Guid productId);

        Task<WishlistItem?> GetWishlistItemByIdAsync(
            Guid wishlistItemId);

        #endregion

        #region Validation

        Task<bool> ExistsAsync(
            Guid wishlistId,
            Guid productId);

        #endregion

        #region Update

        Task UpdateAsync(Wishlist wishlist);

        #endregion

        #region Delete

        Task RemoveWishlistItemAsync(WishlistItem wishlistItem);

        Task ClearWishlistAsync(Wishlist wishlist);

        #endregion

        #region Save

        Task SaveChangesAsync();

        #endregion
    }
}