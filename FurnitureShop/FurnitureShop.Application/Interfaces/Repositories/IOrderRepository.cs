using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<Order?> GetOrderByIdAsync(Guid orderId, Guid userId);

    }
}
