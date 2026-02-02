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
        Task<List<ShippingAddress>> GetByUserIdAsync(Guid userId);
        Task<ShippingAddress?> GetByIdAsync(Guid id);
        Task RemoveAsync(ShippingAddress address);
    }
}
