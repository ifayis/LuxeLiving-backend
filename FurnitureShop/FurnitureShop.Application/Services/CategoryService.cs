using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request)
        {
            if (await _repository.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException("Category already exists");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsActive = true
            };

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return Map(category);
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            return (await _repository.GetAllAsync())
                .Select(Map)
                .ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : Map(category);
        }

        public async Task<ApiResponse<object>> DeleteByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return ApiResponse<object>.Fail(
                    ErrorMessages.InvalidId,
                    400
                    );
            }

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return ApiResponse<object>.Fail(
                    ErrorMessages.NotFound,
                    400);
            }

            await _repository.DeleteAsync(entity);

            return ApiResponse<object>.Success(
                null,
                ResponseMessages.CategoryDeleted,
                200);
        }

        public async Task<ApiResponse<object>> DeleteAllAsync()
        {
            await _repository.DeleteAllAsync();

            return ApiResponse<object>.Success(
                null,
                ResponseMessages.CategoriesDeleted,
                200);
        }
        private static CategoryResponseDto Map(Category c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            IsActive = c.IsActive
        };
    }
}
