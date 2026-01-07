using System;
using System.Threading.Tasks;
using FurnitureShop.Application.Interfaces.Repositories;
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

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetByIdAsync(Guid cartId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task AddAsync(Cart cart)
        {
            _context.Carts.Add(cart);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
        }

    }
}
