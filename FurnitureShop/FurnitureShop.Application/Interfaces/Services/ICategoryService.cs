using FurnitureShop.Application.DTOs.Category;

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
