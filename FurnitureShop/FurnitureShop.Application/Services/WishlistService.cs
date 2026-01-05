using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task AddAsync(Guid userId, AddToWishlistRequestDto request)
        {
            if (request == null || request.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid input");

            var productExists = await _wishlistRepository.ProductExistsAsync(request.ProductId);
            if (!productExists)
                throw new ArgumentException("Invalid product");

            var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                await _wishlistRepository.AddAsync(wishlist);
            }

            if (wishlist.Items.Any(i => i.ProductId == request.ProductId))
                throw new InvalidOperationException("Item already exists");

            wishlist.Items.Add(new WishlistItem
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId
            });

            await _wishlistRepository.SaveChangesAsync();
        }

        public async Task<WishlistResponseDto?> GetMyWishlistAsync(Guid userId)
        {
            var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);
            if (wishlist == null) return null;

            return Map(wishlist);
        }

        public async Task<WishlistResponseDto?> GetByIdAsync(Guid id)
        {
            var wishlist = await _wishlistRepository.GetByIdAsync(id);
            return wishlist == null ? null : Map(wishlist);
        }

        public async Task RemoveItemAsync(Guid userId, Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentException("Invalid item id");

            var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);
            if (wishlist == null)
                throw new InvalidOperationException("Wishlist not found");

            var item = wishlist.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new InvalidOperationException("Wishlist item not found");

            _wishlistRepository.RemoveItem(item);
            await _wishlistRepository.SaveChangesAsync();
        }

        public async Task ClearAsync(Guid userId)
        {
            var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);
            if (wishlist == null)
                throw new InvalidOperationException("Wishlist not found");

            _wishlistRepository.RemoveAll(wishlist);
            await _wishlistRepository.SaveChangesAsync();
        }

        private static WishlistResponseDto Map(Wishlist wishlist)
        {
            return new WishlistResponseDto
            {
                Id = wishlist.Id,
                Items = wishlist.Items.Select(i => new WishlistItemResponseDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    ImageUrl = i.Product.ImageUrl
                }).ToList()
            };
        }
    }
}
