using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Category;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateAsync(
            CreateCategoryRequestDto request);

        Task<CategoryResponseDto> UpdateAsync(
            Guid categoryId,
            UpdateCategoryRequestDto request);

        Task DeleteAsync(Guid categoryId);
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<List<CategoryResponseDto>> GetActiveAsync();
        Task<CategoryResponseDto?> GetByIdAsync(Guid categoryId);
        Task<CategoryResponseDto?> GetBySlugAsync(string slug);
    }
}