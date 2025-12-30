using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetByUserIdAsync(Guid userId);
        Task<Wishlist?> GetByIdAsync(Guid id);
        Task AddAsync(Wishlist wishlist);
        Task SaveChangesAsync();
        void RemoveItem(WishlistItem item);
        void RemoveAll(Wishlist wishlist);
    }
}
