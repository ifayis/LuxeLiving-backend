using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Wishlist wishlist);
        void AddItem(WishlistItem item);
        void RemoveItem(WishlistItem item);
        void RemoveAll(Wishlist wishlist);
        Task SaveChangesAsync();
    }
}
