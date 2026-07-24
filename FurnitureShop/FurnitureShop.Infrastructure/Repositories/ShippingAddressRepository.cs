using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Repositories
{
    public class ShippingAddressRepository
        : IShippingAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public ShippingAddressRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create

        public async Task AddAsync(
            ShippingAddress address)
        {
            await _context.ShippingAddresses
                .AddAsync(address);
        }

        #endregion

        #region Read

        public async Task<ShippingAddress?> GetByIdAsync(
            Guid addressId)
        {
            return await _context.ShippingAddresses
                .FirstOrDefaultAsync(x =>
                    x.Id == addressId);
        }

        public async Task<List<ShippingAddress>> GetByUserIdAsync(
            Guid userId)
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
                    x.Id == addressId);
        }

        public async Task<ShippingAddress?> GetDefaultAsync(
            Guid userId)
        {
            return await _context.ShippingAddresses
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.IsDefault);
        }

        #endregion

        #region Validation

        public async Task<bool> ExistsAsync(
            Guid userId,
            Guid addressId)
        {
            return await _context.ShippingAddresses
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.Id == addressId);
        }

        #endregion

        #region Update

        public Task UpdateAsync(
            ShippingAddress address)
        {
            address.UpdatedAt = DateTime.UtcNow;

            _context.ShippingAddresses.Update(address);

            return Task.CompletedTask;
        }

        #endregion

        #region Delete

        public Task DeleteAsync(
            ShippingAddress address)
        {
            _context.ShippingAddresses.Remove(address);

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