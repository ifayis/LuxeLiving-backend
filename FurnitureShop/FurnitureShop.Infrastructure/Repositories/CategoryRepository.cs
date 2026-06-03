using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Enitities;
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

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .ToListAsync();
        }
        public async Task<bool> ExistsByNameExceptIdAsync(
            string name,
            Guid categoryId)
        {
            return await _context.Categories.AnyAsync(c =>
                c.Id != categoryId &&
                c.Name.ToLower() == name.ToLower());
        }

        public async Task DeleteAllAsync()
        {
            _context.Categories.RemoveRange(_context.Categories);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsByNameExceptIdAsync(string v, object categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
