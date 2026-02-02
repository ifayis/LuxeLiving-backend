using FurnitureShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IShippingAddressRepository
    {
        Task<ShippingAddress?> GetByUserIdAsync(Guid userId);
        Task AddAsync(ShippingAddress address);
        Task SaveChangesAsync();
    }
}
