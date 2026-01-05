using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Products
                .AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
