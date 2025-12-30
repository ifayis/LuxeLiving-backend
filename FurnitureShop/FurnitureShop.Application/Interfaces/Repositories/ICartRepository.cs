using System;
using System.Threading.Tasks;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart?> GetByIdAsync(Guid cartId);
        Task AddAsync(Cart cart);
        Task SaveChangesAsync();
    }
}
