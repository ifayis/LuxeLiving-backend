using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
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

        public async Task DeleteByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            await _repository.DeleteAsync(category);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            await _repository.DeleteAllAsync();
            await _repository.SaveChangesAsync();
        }

        private static CategoryResponseDto Map(Category c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            IsActive = c.IsActive
        };
    }
}
