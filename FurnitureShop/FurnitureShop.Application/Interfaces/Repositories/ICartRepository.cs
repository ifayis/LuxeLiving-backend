using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart?> GetByIdAsync(Guid cartId);
        Task<bool> ExistsAsync(Guid userId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Cart cart);

        
        Task<CartItem?> GetCartItemAsync(
            Guid cartId,
            Guid productId);
        Task AddCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
        Task<int> GetCartItemCountAsync(Guid cartId);

        
        Task ClearCartAsync(Guid userId);
        Task SaveChangesAsync();
    }
}