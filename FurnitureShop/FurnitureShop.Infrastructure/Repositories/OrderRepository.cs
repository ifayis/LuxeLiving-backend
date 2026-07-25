using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        #endregion

        #region Read

        public async Task<Order?> GetByIdAsync(
            Guid orderId)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.ShippingAddress)
                .FirstOrDefaultAsync(x =>
                    x.Id == orderId);
        }

        public async Task<Order?> GetByOrderNumberAsync(
            string orderNumber)
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
                    x.UserId == userId);
        }

        public async Task<bool> ExistsOrderNumberAsync(
            string orderNumber)
        {
            return await _context.Orders
                .AnyAsync(x =>
                    x.OrderNumber == orderNumber);
        }

        public async Task<List<Order>> GetByUserIdAsync(
            Guid userId)
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

        #endregion

        #region Update

        public Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);

            return Task.CompletedTask;
        }

        #endregion

        #region Analytics

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

        #endregion

        #region Persistence

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}