using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Checkout;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Application.Services
{
    public partial class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShippingAddressRepository _shippingAddressRepository;
        private readonly IOrderRepository _orderRepository;

        public CheckoutService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IShippingAddressRepository shippingAddressRepository,
            IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _shippingAddressRepository = shippingAddressRepository;
            _orderRepository = orderRepository;
        }

        public async Task<CheckoutSummaryDto> GetSummaryAsync(
            Guid userId)
        {
            var cart = await _cartRepository
                .GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException(
                    ErrorMessages.CartEmpty);
            }

            var checkoutItems = new List<CheckoutItemDto>();

            foreach (var cartItem in cart.Items)
            {
                var product = await _productRepository
                    .GetByIdAsync(cartItem.ProductId);

                if (product == null)
                {
                    throw new InvalidOperationException(
                        ErrorMessages.ProductNotFound);
                }

                if (!product.IsActive)
                {
                    throw new InvalidOperationException(
                        $"{product.Name} is unavailable.");
                }

                if (product.StockQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException(
                        $"{product.Name} has only {product.StockQuantity} item(s) remaining.");
                }

                checkoutItems.Add(new CheckoutItemDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    UnitPrice = product.Price,
                    Quantity = cartItem.Quantity
                });
            }

            var defaultAddress =
                await _shippingAddressRepository
                    .GetDefaultAsync(userId);

            return new CheckoutSummaryDto
            {
                Items = checkoutItems,

                ShippingAddress =
                    defaultAddress == null
                        ? null
                        : new Application.DTOs.ShippingAddress
                            .ShippingAddressResponseDto
                        {
                            Id = defaultAddress.Id,
                            FullName = defaultAddress.FullName,
                            PhoneNumber = defaultAddress.PhoneNumber,
                            AddressLine1 = defaultAddress.AddressLine1,
                            AddressLine2 = defaultAddress.AddressLine2,
                            City = defaultAddress.City,
                            State = defaultAddress.State,
                            Country = defaultAddress.Country,
                            PinCode = defaultAddress.PinCode,
                            AddressType = defaultAddress.AddressType,
                            IsDefault = defaultAddress.IsDefault,
                            CreatedAt = defaultAddress.CreatedAt,
                            UpdatedAt = defaultAddress.UpdatedAt
                        },

                TotalItems = checkoutItems.Sum(x => x.Quantity),

                SubTotal = checkoutItems.Sum(x => x.SubTotal),

                ShippingCharge = 0,

                Discount = 0,

                Tax = 0,

                GrandTotal = checkoutItems.Sum(x => x.SubTotal)
            };
        }

        public async Task<PaymentResponseDto> CheckoutAsync(
            Guid userId,
            CheckoutRequestDto request)
        {
            var cart = await _cartRepository
                .GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException(
                    ErrorMessages.CartEmpty);
            }

            var shippingAddress =
                await _shippingAddressRepository
                    .GetUserAddressAsync(
                        userId,
                        request.ShippingAddressId);

            if (shippingAddress == null)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.AddressNotFound);
            }

            decimal grandTotal = 0;

            var orderItems = new List<OrderItem>();

            foreach (var cartItem in cart.Items)
            {
                var product = await _productRepository
                    .GetByIdAsync(cartItem.ProductId);

                if (product == null)
                {
                    throw new InvalidOperationException(
                        ErrorMessages.ProductNotFound);
                }

                if (!product.IsActive)
                {
                    throw new InvalidOperationException(
                        $"{product.Name} is unavailable.");
                }

                if (product.StockQuantity < cartItem.Quantity)
                {
                    throw new InvalidOperationException(
                        $"{product.Name} has only {product.StockQuantity} item(s) remaining.");
                }

                grandTotal +=
                    product.Price * cartItem.Quantity;

                var lineTotal =
                    product.Price * cartItem.Quantity;

                grandTotal += lineTotal;

                orderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),

                    ProductId = product.Id,

                    ProductName = product.Name,

                    ProductImageUrl = product.ImageUrl,

                    Quantity = cartItem.Quantity,

                    UnitPrice = product.Price,

                    LineTotal = lineTotal
                });

                product.StockQuantity -= cartItem.Quantity;
            }

            string orderNumber;

            do
            {
                orderNumber =
                    OrderNumberGenerator.Generate();
            }
            while (await _orderRepository
                .ExistsOrderNumberAsync(orderNumber));

            var order = new Order
            {
                Id = Guid.NewGuid(),

                OrderNumber = orderNumber,

                UserId = userId,

                ShippingAddressId = shippingAddress.Id,

                Status = OrderStatus.Pending,

                PaymentMethod = request.PaymentMethod,

                SubTotal = grandTotal,

                ShippingCharge = 0,

                Discount = 0,

                Tax = 0,

                GrandTotal = grandTotal,

                Items = orderItems,

                CreatedAt = DateTime.UtcNow,
            };

            await _orderRepository.AddAsync(order);

            await _productRepository.SaveChangesAsync();

            await _cartRepository.ClearCartAsync(userId);

            await _cartRepository.SaveChangesAsync();

            await _orderRepository.SaveChangesAsync();

            return new PaymentResponseDto
            {
                OrderId = order.Id,

                OrderNumber = order.OrderNumber,

                Amount = grandTotal,

                PaymentMethod = request.PaymentMethod,

                Message =
                    request.PaymentMethod ==
                    PaymentMethod.CashOnDelivery
                        ? "Order placed successfully."
                        : "Proceed to online payment."
            };
        }
    }
}