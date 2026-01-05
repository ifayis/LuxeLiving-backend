using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IWishlistRepository
    {
        Task AddAsync(Wishlist wishlist);
        Task<Wishlist?> GetByUserIdAsync(Guid userId);
        Task<Wishlist?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        void RemoveItem(WishlistItem item);
        void RemoveAll(Wishlist wishlist);
    }
}
