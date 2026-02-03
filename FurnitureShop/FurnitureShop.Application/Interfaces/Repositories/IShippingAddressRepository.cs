using FurnitureShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IShippingAddressRepository
    {
        Task AddAsync(ShippingAddress address);
        Task<ShippingAddress?> GetByIdAsync(Guid id);
        Task<List<ShippingAddress>> GetByUserIdAsync(Guid userId);
        Task DeleteAsync(ShippingAddress address);
        Task SaveChangesAsync();
    }
}
