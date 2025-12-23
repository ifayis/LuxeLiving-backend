using FurnitureShop.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces
{
    public interface IOrderService
    {
        Task CheckoutAsync(Guid userId, CheckoutRequestDto request);
    }
}
