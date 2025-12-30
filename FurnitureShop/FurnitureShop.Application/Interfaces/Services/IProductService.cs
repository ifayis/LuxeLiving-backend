using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId);
        Task CreateAsync(CreateProductRequestDto request);
        Task<ApiResponse<IEnumerable<ProductResponseDto>>> GetAllProducts();
    }
}
