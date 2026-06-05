using FurnitureShop.Application.DTOs.ShippingAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IShippingAddressService
    {
        Task AddAsync(Guid userId, ShippingAddressRequestDto dto);
        Task<List<ShippingAddressResponseDto>> GetMyAsync(Guid userId);
        Task UpdateAsync(Guid userId, Guid addressId, ShippingAddressRequestDto dto);
        Task DeleteAsync(Guid userId, Guid addressId);
        Task SetDefaultAsync(Guid userId, Guid addressId);
    }
}
