using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);
        Task AddCartItemAsync(CartItem item);
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart?> GetByIdAsync(Guid cartId);
        Task AddAsync(Cart cart);
        Task SaveChangesAsync();
        void Clear(Cart cart);
        Task ClearCartAsync(Guid userId);
    }
}
