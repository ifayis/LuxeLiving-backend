using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Category;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request);
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, UpdateCategoryRequestDto request);
        Task<ApiResponse<object>> DeleteByIdAsync(Guid id);
        Task<ApiResponse<object>> DeleteAllAsync();
    }
}
