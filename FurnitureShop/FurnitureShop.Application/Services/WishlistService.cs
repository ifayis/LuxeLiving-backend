using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task AddToWishlistAsync(Guid userId, AddToWishlistRequestDto request)
        {
            var exists = await _wishlistRepository.ExistsAsync(userId, request.ProductId);
            if (exists) return;

            var wishlist = new Wishlist
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = request.ProductId
            };

            await _wishlistRepository.AddAsync(wishlist);
        }
    }
}
