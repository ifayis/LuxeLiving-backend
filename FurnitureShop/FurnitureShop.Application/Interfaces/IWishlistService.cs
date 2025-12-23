using FurnitureShop.Application.DTOs.Wishlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces
{
    public interface IWishlistService
    {
        Task AddToWishlistAsync(Guid userId, AddToWishlistRequestDto request);
    }
}
