using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Infrastructure.Data;
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

        public async Task<ShippingAddress?> GetByUserIdAsync(Guid userId)
        {
            return await _context.ShippingAddresses
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(ShippingAddress address)
        {
            _context.ShippingAddresses.Add(address);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
