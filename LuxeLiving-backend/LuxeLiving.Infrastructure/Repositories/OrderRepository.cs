using LuxeLiving.Application.Interfaces.Repositories;
using LuxeLiving.Domain.Enitities;
using LuxeLiving.Domain.Enums;
using LuxeLiving.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LuxeLiving.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<Order?> GetByIdAsync(
            Guid orderId)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.ShippingAddress)
                .FirstOrDefaultAsync(x =>
                    x.Id == orderId);
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.ShippingAddress)
                .FirstOrDefaultAsync(x =>
                    x.OrderNumber == orderNumber);
        }

        public async Task<Order?> GetByIdAsync(
            Guid orderId,
            Guid userId)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.ShippingAddress)
                .FirstOrDefaultAsync(x =>
                    x.Id == orderId &&
                    x.UserId == userId
                );
        }

        public async Task<bool> ExistsOrderNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .AnyAsync(x =>
                    x.OrderNumber == orderNumber);
        }

        public async Task<List<Order>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.ShippingAddress)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.ShippingAddress)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);

            return Task.CompletedTask;
        }

        public async Task<bool> HasPurchasedProductAsync(
            Guid userId,
            Guid orderId,
            Guid productId)
        {
            return await _context.Orders
                .AnyAsync(o =>
                    o.Id == orderId &&
                    o.UserId == userId &&
                    o.Status == OrderStatus.Delivered &&
                    o.Items.Any(i => i.ProductId == productId)
                );
        }

        public async Task<int> GetTotalProductsPurchasedAsync()
        {
            return await _context.OrderItems
                .SumAsync(x => x.Quantity);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .Where(x =>
                    x.Status ==
                    Domain.Enums.OrderStatus.Delivered)
                .SumAsync(x => x.GrandTotal);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}