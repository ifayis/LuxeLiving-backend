using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces
{
    public interface IWishlistRepository
    {
        Task<bool> ExistsAsync(Guid userId, Guid productId);
        Task AddAsync(Wishlist wishlist);
    }
}
