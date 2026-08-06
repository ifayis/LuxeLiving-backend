using LuxeLiving.Application.Interfaces.Repositories;
using LuxeLiving.Domain.Entities;
using LuxeLiving.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LuxeLiving.Infrastructure.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public ShippingAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ShippingAddress address)
        {
            await _context.ShippingAddresses
                .AddAsync(address);
        }

        public async Task<ShippingAddress?> GetByIdAsync(Guid addressId)
        {
            return await _context.ShippingAddresses
                .FirstOrDefaultAsync(x =>
                    x.Id == addressId);
        }

        public async Task<List<ShippingAddress>> GetByUserIdAsync(Guid userId)
        {
            return await _context.ShippingAddresses
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.IsDefault)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ShippingAddress?> GetUserAddressAsync(
            Guid userId,
            Guid addressId)
        {
            return await _context.ShippingAddresses
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.Id == addressId
                );
        }

        public async Task<ShippingAddress?> GetDefaultAsync(Guid userId)
        {
            return await _context.ShippingAddresses
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.IsDefault
                );
        }

        public async Task<bool> ExistsAsync(
            Guid userId,
            Guid addressId)
        {
            return await _context.ShippingAddresses
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.Id == addressId
                );
        }

        public Task UpdateAsync(ShippingAddress address)
        {
            address.UpdatedAt = DateTime.UtcNow;

            _context.ShippingAddresses.Update(address);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(ShippingAddress address)
        {
            _context.ShippingAddresses.Remove(address);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}