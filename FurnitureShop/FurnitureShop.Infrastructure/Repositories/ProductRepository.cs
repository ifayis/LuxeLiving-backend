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

        #region Create

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        #endregion

        #region Read

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> GetBySlugAsync(string slug)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.SKU == sku);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetActiveAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(Guid categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x =>
                    x.CategoryId == categoryId &&
                    x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Product>> GetFeaturedProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x =>
                    x.IsActive &&
                    x.IsFeatured)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetNewArrivalProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x =>
                    x.IsActive &&
                    x.IsNewArrival)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetBestSellerProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x =>
                    x.IsActive &&
                    x.IsBestSeller)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim().ToLower();

            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x =>
                    x.IsActive &&
                    (
                        x.Name.ToLower().Contains(keyword) ||
                        x.Description.ToLower().Contains(keyword) ||
                        x.SKU.ToLower().Contains(keyword)
                    ))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        #endregion

        #region Validation

        public async Task<bool> ExistsByNameAsync(string name)
        {
            name = name.Trim().ToLower();

            return await _context.Products
                .AnyAsync(x =>
                    x.Name.ToLower() == name);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            Guid excludeProductId)
        {
            name = name.Trim().ToLower();

            return await _context.Products
                .AnyAsync(x =>
                    x.Id != excludeProductId &&
                    x.Name.ToLower() == name);
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            slug = slug.Trim().ToLower();

            return await _context.Products
                .AnyAsync(x => x.Slug == slug);
        }

        public async Task<bool> ExistsBySkuAsync(string sku)
        {
            sku = sku.Trim().ToUpper();

            return await _context.Products
                .AnyAsync(x => x.SKU == sku);
        }

        #endregion

        #region Update

        public Task UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);

            return Task.CompletedTask;
        }

        #endregion

        #region Delete

        public Task DeleteAsync(Product product)
        {
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);

            return Task.CompletedTask;
        }

        #endregion

        #region Save

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}