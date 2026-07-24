using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Entities;
using System.Text.RegularExpressions;

namespace FurnitureShop.Application.Services
{
    public partial class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDto> CreateAsync(
            CreateCategoryRequestDto request)
        {
            var name = request.Name.Trim();

            if (await _categoryRepository.ExistsByNameAsync(name))
                throw new InvalidOperationException(
                    ErrorMessages.CategoryAlreadyExists);

            var slug = await GenerateUniqueSlugAsync(name);

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = name,
                Slug = slug,
                Description = request.Description?.Trim(),
                ImageUrl = request.ImageUrl?.Trim(),
                DisplayOrder = request.DisplayOrder,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category);

            await _categoryRepository.SaveChangesAsync();

            return Map(category);
        }

        private async Task<string> GenerateUniqueSlugAsync(
            string name)
        {
            var slug = GenerateSlug(name);

            var originalSlug = slug;

            var count = 1;

            while (await _categoryRepository.ExistsBySlugAsync(slug))
            {
                slug = $"{originalSlug}-{count++}";
            }

            return slug;
        }

        private static string GenerateSlug(string text)
        {
            text = text.Trim().ToLowerInvariant();

            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            text = Regex.Replace(text, @"\s+", "-");

            text = Regex.Replace(text, @"-+", "-");

            return text;
        }

        public async Task<CategoryResponseDto> UpdateAsync(
    Guid categoryId,
    UpdateCategoryRequestDto request)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                throw new KeyNotFoundException(
                    ErrorMessages.CategoryNotFound);

            var name = request.Name.Trim();

            if (await _categoryRepository.ExistsByNameAsync(
                name,
                categoryId))
            {
                throw new InvalidOperationException(
                    ErrorMessages.CategoryAlreadyExists);
            }

            if (!string.Equals(
                    category.Name,
                    name,
                    StringComparison.OrdinalIgnoreCase))
            {
                category.Slug = await GenerateUniqueSlugAsync(name);
            }

            category.Name = name;
            category.Description = request.Description?.Trim();
            category.ImageUrl = request.ImageUrl?.Trim();
            category.DisplayOrder = request.DisplayOrder;
            category.IsActive = request.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryRepository.UpdateAsync(category);

            await _categoryRepository.SaveChangesAsync();

            return Map(category);
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                throw new KeyNotFoundException(
                    ErrorMessages.CategoryNotFound);

            await _categoryRepository.DeleteAsync(category);

            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories
                .Select(Map)
                .ToList();
        }

        public async Task<List<CategoryResponseDto>> GetActiveAsync()
        {
            var categories = await _categoryRepository.GetActiveAsync();

            return categories
                .Select(Map)
                .ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                return null;

            return Map(category);
        }

        public async Task<CategoryResponseDto?> GetBySlugAsync(string slug)
        {
            var category = await _categoryRepository.GetBySlugAsync(slug);

            if (category == null)
                return null;

            return Map(category);
        }

        private static CategoryResponseDto Map(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                DisplayOrder = category.DisplayOrder,
                ProductCount = category.Products?.Count ?? 0,
                IsActive = category.IsActive
            };
        }
    }
}
