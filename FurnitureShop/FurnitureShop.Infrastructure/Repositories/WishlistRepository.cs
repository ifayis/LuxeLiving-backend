using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create

        public async Task AddAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
        }

        public async Task AddWishlistItemAsync(
            WishlistItem wishlistItem)
        {
            await _context.WishlistItems.AddAsync(wishlistItem);
        }

        #endregion

        #region Read

        public async Task<Wishlist?> GetByIdAsync(Guid wishlistId)
        {
            return await _context.Wishlists
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == wishlistId);
        }

        public async Task<Wishlist?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Wishlists
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<WishlistItem?> GetWishlistItemAsync(
            Guid wishlistId,
            Guid productId)
        {
            return await _context.WishlistItems
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x =>
                    x.WishlistId == wishlistId &&
                    x.ProductId == productId);
        }

        public async Task<WishlistItem?> GetWishlistItemByIdAsync(
            Guid wishlistItemId)
        {
            return await _context.WishlistItems
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x =>
                    x.Id == wishlistItemId);
        }

        #endregion

        #region Validation

        public async Task<bool> ExistsAsync(
            Guid wishlistId,
            Guid productId)
        {
            return await _context.WishlistItems
                .AnyAsync(x =>
                    x.WishlistId == wishlistId &&
                    x.ProductId == productId);
        }

        #endregion

        #region Update

        public Task UpdateAsync(Wishlist wishlist)
        {
            wishlist.UpdatedAt = DateTime.UtcNow;

            _context.Wishlists.Update(wishlist);

            return Task.CompletedTask;
        }

        #endregion

        #region Delete

        public Task RemoveWishlistItemAsync(
            WishlistItem wishlistItem)
        {
            _context.WishlistItems.Remove(wishlistItem);

            return Task.CompletedTask;
        }

        public Task ClearWishlistAsync(Wishlist wishlist)
        {
            _context.WishlistItems.RemoveRange(wishlist.Items);

            wishlist.UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        #endregion

        #region Save

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}