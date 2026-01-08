using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _WishlistRepository;
        private readonly IProductRepository _productRepository;

        public WishlistService(IWishlistRepository WishlistRepository, IProductRepository productRepository)
        {
            _WishlistRepository = WishlistRepository;
            _productRepository = productRepository;

        }

        public async Task AddAsync(Guid userId, AddToWishlistRequestDto request)
        {
            if (request == null || request.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid wishlist request");

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new ArgumentException("Product does not exist");

            var wishlist = await _WishlistRepository.GetByUserIdAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                await _WishlistRepository.AddAsync(wishlist);
                await _WishlistRepository.SaveChangesAsync();
            }

            if (wishlist.Items.Any(i => i.ProductId == request.ProductId))
                throw new InvalidOperationException("Product already in wishlist");

            wishlist.Items.Add(new WishlistItem
            {
                Id = Guid.NewGuid(),
                WishlistId = wishlist.Id,
                ProductId = request.ProductId
            });

            await _WishlistRepository.SaveChangesAsync();
        }

        public async Task<WishlistResponseDto?> GetMyAsync(Guid userId)
        {
            var wishlist = await _WishlistRepository.GetByUserIdAsync(userId);
            return wishlist == null ? null : Map(wishlist);
        }

        public async Task RemoveItemAsync(Guid userId, Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentException("Invalid item id");

            var wishlist = await _WishlistRepository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException("Wishlist not found");

            var item = wishlist.Items.FirstOrDefault(i => i.Id == itemId)
                ?? throw new InvalidOperationException("Wishlist item not found");

            _WishlistRepository.RemoveItem(item);
            await _WishlistRepository.SaveChangesAsync();
        }

        public async Task ClearAsync(Guid userId)
        {
            var wishlist = await _WishlistRepository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException("Wishlist not found");

            _WishlistRepository.RemoveAll(wishlist);
            await _WishlistRepository.SaveChangesAsync();
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
