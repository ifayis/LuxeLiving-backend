using FurnitureShop.Application.DTOs.Wishlist;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IWishlistService
    {
        #region Create

        Task<WishlistResponseDto> AddAsync(
            Guid userId,
            AddToWishlistRequestDto request);

        #endregion

        #region Read

        Task<WishlistResponseDto?> GetMyWishlistAsync(
            Guid userId);

        Task<WishlistResponseDto?> GetWishlistByUserIdAsync(
            Guid userId);

        Task<WishlistResponseDto?> GetWishlistByIdAsync(
            Guid wishlistId);

        #endregion

        #region Update

        Task MoveToCartAsync(
            Guid userId);

        #endregion

        #region Delete

        Task RemoveItemAsync(
            Guid userId,
            Guid wishlistItemId);

        Task ClearAsync(
            Guid userId);

        #endregion
    }
}