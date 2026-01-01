using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request);
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto?> GetByIdAsync(Guid id);
        Task<bool> DeleteByIdAsync(Guid id);
        Task DeleteAllAsync();
    }
}
