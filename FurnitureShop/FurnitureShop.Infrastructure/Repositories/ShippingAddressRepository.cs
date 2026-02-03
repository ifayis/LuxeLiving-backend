using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Infrastructure.Repositories
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
            _context.ShippingAddresses.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task<ShippingAddress?> GetByIdAsync(Guid id)
        {
            return await _context.ShippingAddresses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ShippingAddress>> GetByUserIdAsync(Guid userId)
        {
            return await _context.ShippingAddresses
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(ShippingAddress address)
        {
            _context.ShippingAddresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
