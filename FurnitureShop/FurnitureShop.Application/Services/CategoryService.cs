using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository;

        public CategoryService(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request, Guid categoryId)
        {
            if (await _CategoryRepository
                .ExistsByNameExceptIdAsync(
                    request.Name.Trim(),
                    categoryId))
            {
                throw new InvalidOperationException(
                    "Category already exists");
            }
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                IsActive = true
            };

            await _CategoryRepository.AddAsync(category);
            await _CategoryRepository.SaveChangesAsync();

            return Map(category);
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            return (await _CategoryRepository.GetAllAsync())
                .Select(Map)
                .ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
        {
            var category = await _CategoryRepository.GetByIdAsync(id);
            return category == null ? null : Map(category);
        }

        public async Task<bool> UpdateAsync(Guid categoryId, UpdateCategoryRequestDto request)
        {
            var category = await _CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return false;

            if (await _CategoryRepository.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException("Category already exists");

            category.Name = request.Name.Trim();
            category.IsActive = request.IsActive;

            await _CategoryRepository.SaveChangesAsync();
            return true;
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

            var entity = await _CategoryRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return ApiResponse<object>.Fail(
                    ErrorMessages.NotFound,
                    400);
            }

            entity.IsActive = false;

            await _CategoryRepository.SaveChangesAsync();
            
            return ApiResponse<object>.Success(
                null,
                ResponseMessages.CategoryDeleted,
                200);
        }

        public async Task<ApiResponse<object>> DeleteAllAsync()
        {
            await _CategoryRepository.DeleteAllAsync();
            await _CategoryRepository.SaveChangesAsync();

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

        public Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
