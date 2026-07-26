using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Order;
using FurnitureShop.Application.DTOs.ShippingAddress;
using FurnitureShop.Application.Interfaces.Common;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Application.Services
{
    public partial class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        #region Customer

        public async Task<List<OrderResponseDto>>
            GetMyOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository
                .GetByUserIdAsync(userId);

            return orders
                .Select(Map)
                .ToList();
        }

        public async Task<OrderResponseDto?>
            GetMyOrderAsync(
                Guid userId,
                Guid orderId)
        {
            var order = await _orderRepository
                .GetByIdAsync(orderId, userId);

            return order == null
                ? null
                : Map(order);
        }

        #endregion

        #region Admin

        public async Task<List<OrderResponseDto>>
            GetAllOrdersAsync()
        {
            var orders = await _orderRepository
                .GetAllAsync();

            return orders
                .Select(Map)
                .ToList();
        }

        public async Task<List<OrderResponseDto>>
            GetOrdersByUserAsync(Guid userId)
        {
            var orders = await _orderRepository
                .GetByUserIdAsync(userId);

            return orders
                .Select(Map)
                .ToList();
        }

        public async Task<OrderResponseDto?>
            GetOrderAsync(Guid orderId)
        {
            var order = await _orderRepository
                .GetByIdAsync(orderId);

            return order == null
                ? null
                : Map(order);
        }

        #endregion

        private static OrderResponseDto Map(
            Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.Id,

                OrderNumber = order.OrderNumber,

                Status = order.Status,

                PaymentMethod = order.PaymentMethod,

                SubTotal = order.SubTotal,

                ShippingCharge = order.ShippingCharge,

                Discount = order.Discount,

                Tax = order.Tax,

                GrandTotal = order.GrandTotal,

                CancellationReason = order.CancellationReason,

                CancelledAt = order.CancelledAt,

                CreatedAt = order.CreatedAt,

                ShippingAddress = new ShippingAddressResponseDto
                {
                    Id = order.ShippingAddress.Id,

                    FullName = order.ShippingAddress.FullName,

                    PhoneNumber = order.ShippingAddress.PhoneNumber,

                    AddressLine1 = order.ShippingAddress.AddressLine1,

                    AddressLine2 = order.ShippingAddress.AddressLine2,

                    City = order.ShippingAddress.City,

                    State = order.ShippingAddress.State,

                    Country = order.ShippingAddress.Country,

                    PinCode = order.ShippingAddress.PinCode,

                    AddressType = order.ShippingAddress.AddressType,

                    IsDefault = order.ShippingAddress.IsDefault
                },

                Items = order.Items
                    .Select(item => new OrderItemResponseDto
                    {
                        ProductId = item.ProductId,

                        ProductName = item.ProductName,

                        ProductImageUrl = item.ProductImageUrl,

                        UnitPrice = item.UnitPrice,

                        Quantity = item.Quantity,

                        LineTotal = item.LineTotal
                    })
                    .ToList()
            };
        }

        public async Task<OrderResponseDto?> CancelOrderAsync(
            Guid userId,
            Guid orderId,
            CancelOrderRequestDto request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order =
                    await _orderRepository.GetByIdAsync(
                        orderId,
                        userId);

                if (order == null)
                {
                    return null;
                }

                if (order.Status == OrderStatus.Delivered)
                {
                    throw new InvalidOperationException(
                        "Delivered orders cannot be cancelled.");
                }

                if (order.Status == OrderStatus.Cancelled)
                {
                    throw new InvalidOperationException(
                        "Order is already cancelled.");
                }

                if (order.Status == OrderStatus.Refunded)
                {
                    throw new InvalidOperationException(
                        "Refunded orders cannot be cancelled.");
                }

                foreach (var item in order.Items)
                {
                    var product =
                        await _productRepository.GetByIdAsync(
                            item.ProductId);

                    if (product == null)
                    {
                        continue;
                    }

                    product.StockQuantity += item.Quantity;
                }

                order.Status = OrderStatus.Cancelled;

                order.CancellationReason = string.IsNullOrWhiteSpace(request.Reason)
                    ? null
                    : request.Reason.Trim();

                order.CancelledAt = DateTime.UtcNow;

                order.UpdatedAt = DateTime.UtcNow;

                await _orderRepository.UpdateAsync(order);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return Map(order);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<OrderResponseDto> UpdateStatusAsync(
            Guid orderId,
            UpdateOrderStatusRequestDto request)
        {
            var order = await _orderRepository
                .GetByIdAsync(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.OrderNotFound);
            }

            ValidateStatusTransition(
                order.Status,
                request.Status);

            order.Status = request.Status;

            await _orderRepository.UpdateAsync(order);

            await _orderRepository.SaveChangesAsync();

            return Map(order);
        }


        private static void ValidateStatusTransition(
            OrderStatus currentStatus,
            OrderStatus newStatus)
        {
            if (currentStatus == newStatus)
            {
                return;
            }

            switch (currentStatus)
            {
                case OrderStatus.Pending:

                    if (newStatus != OrderStatus.Confirmed &&
                        newStatus != OrderStatus.Cancelled)
                    {
                        throw new InvalidOperationException(
                            "Invalid order status transition.");
                    }

                    break;

                case OrderStatus.Confirmed:

                    if (newStatus != OrderStatus.Processing &&
                        newStatus != OrderStatus.Cancelled)
                    {
                        throw new InvalidOperationException(
                            "Invalid order status transition.");
                    }

                    break;

                case OrderStatus.Processing:

                    if (newStatus != OrderStatus.Shipped)
                    {
                        throw new InvalidOperationException(
                            "Invalid order status transition.");
                    }

                    break;

                case OrderStatus.Shipped:

                    if (newStatus != OrderStatus.OutForDelivery)
                    {
                        throw new InvalidOperationException(
                            "Invalid order status transition.");
                    }

                    break;

                case OrderStatus.OutForDelivery:

                    if (newStatus != OrderStatus.Delivered)
                    {
                        throw new InvalidOperationException(
                            "Invalid order status transition.");
                    }

                    break;

                case OrderStatus.Delivered:

                case OrderStatus.Cancelled:

                case OrderStatus.Refunded:

                    throw new InvalidOperationException(
                        "Order can no longer change status.");
            }
        }

        public async Task<int> GetTotalProductsPurchasedAsync()
        {
            return await _orderRepository
                .GetTotalProductsPurchasedAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _orderRepository
                .GetTotalRevenueAsync();
        }
    }
}