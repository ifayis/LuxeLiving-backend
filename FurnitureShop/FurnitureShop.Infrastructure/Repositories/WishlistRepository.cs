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

        public async Task AddAsync(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
        }

        public void AddItem(WishlistItem item)
        {
            _context.WishlistItems.Add(item);
        }

        public async Task<Wishlist?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Wishlists
                .Include(w => w.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public void RemoveItem(WishlistItem item)
        {
            _context.WishlistItems.Remove(item);
        }

        public void RemoveAll(Wishlist wishlist)
        {
            _context.WishlistItems.RemoveRange(wishlist.Items);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
