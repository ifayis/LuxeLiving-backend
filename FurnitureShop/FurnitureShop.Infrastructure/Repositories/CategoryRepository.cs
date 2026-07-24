using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        #endregion

        #region Read

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> GetBySlugAsync(string slug)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Include(x => x.Products)
                .ToListAsync();
        }

        public async Task<List<Category>> GetActiveAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Include(x => x.Products)
                .ToListAsync();
        }

        #endregion

        #region Validation

        public async Task<bool> ExistsByNameAsync(string name)
        {
            name = name.Trim().ToLower();

            return await _context.Categories
                .AnyAsync(x =>
                    x.Name.ToLower() == name);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            Guid excludeCategoryId)
        {
            name = name.Trim().ToLower();

            return await _context.Categories
                .AnyAsync(x =>
                    x.Id != excludeCategoryId &&
                    x.Name.ToLower() == name);
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            slug = slug.Trim().ToLower();

            return await _context.Categories
                .AnyAsync(x =>
                    x.Slug == slug);
        }

        #endregion

        #region Update

        public Task UpdateAsync(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(category);

            return Task.CompletedTask;
        }

        #endregion

        #region Delete

        public Task DeleteAsync(Category category)
        {
            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(category);

            return Task.CompletedTask;
        }

        #endregion

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}