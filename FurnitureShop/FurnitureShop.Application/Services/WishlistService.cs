using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _repository;

        public WishlistService(IWishlistRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Guid userId, AddToWishlistRequestDto request)
        {
            if (request == null || request.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid product");

            if (!await _repository.ProductExistsAsync(request.ProductId))
                throw new ArgumentException("Product does not exist");

            var wishlist = await _repository.GetByUserIdAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                await _repository.AddAsync(wishlist);
            }

            if (wishlist.Items.Any(i => i.ProductId == request.ProductId))
                throw new InvalidOperationException("Product already in wishlist");

            wishlist.Items.Add(new WishlistItem
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId
            });

            await _repository.SaveChangesAsync();
        }

        public async Task<WishlistResponseDto?> GetMyAsync(Guid userId)
        {
            var wishlist = await _repository.GetByUserIdAsync(userId);
            return wishlist == null ? null : Map(wishlist);
        }

        public async Task RemoveItemAsync(Guid userId, Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentException("Invalid item id");

            var wishlist = await _repository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException("Wishlist not found");

            var item = wishlist.Items.FirstOrDefault(i => i.Id == itemId)
                ?? throw new InvalidOperationException("Wishlist item not found");

            _repository.RemoveItem(item);
            await _repository.SaveChangesAsync();
        }

        public async Task ClearAsync(Guid userId)
        {
            var wishlist = await _repository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException("Wishlist not found");

            _repository.RemoveAll(wishlist);
            await _repository.SaveChangesAsync();
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
