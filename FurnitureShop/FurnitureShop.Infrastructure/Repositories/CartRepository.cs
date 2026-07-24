using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Cart

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Cart?> GetByIdAsync(Guid cartId)
        {
            return await _context.Carts
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == cartId);
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _context.Carts
                .AnyAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(Cart cart)
        {
            cart.CreatedAt = DateTime.UtcNow;
            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastActivityAt = DateTime.UtcNow;

            await _context.Carts.AddAsync(cart);
        }

        public Task UpdateAsync(Cart cart)
        {
            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastActivityAt = DateTime.UtcNow;

            _context.Carts.Update(cart);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Cart cart)
        {
            _context.Carts.Remove(cart);

            return Task.CompletedTask;
        }

        #endregion

        #region Cart Items

        public async Task<CartItem?> GetCartItemAsync(
            Guid cartId,
            Guid productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(x =>
                    x.CartId == cartId &&
                    x.ProductId == productId);
        }

        public async Task AddCartItemAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
        }

        public Task RemoveCartItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);

            return Task.CompletedTask;
        }

        public async Task<int> GetCartItemCountAsync(Guid cartId)
        {
            return await _context.CartItems
                .CountAsync(x => x.CartId == cartId);
        }

        #endregion

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
                return;

            _context.CartItems.RemoveRange(cart.Items);

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastActivityAt = DateTime.UtcNow;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}