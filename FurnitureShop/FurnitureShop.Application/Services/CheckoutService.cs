using FurnitureShop.Application.DTOs.Checkout;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Application.common;

namespace FurnitureShop.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public CheckoutService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<CheckoutResponseDto> GetCheckoutAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
                return new CheckoutResponseDto();

            var items = cart.Items
                .Where(i =>
                    i.Product != null &&
                    i.Product.IsActive)
                .Select(i => new CheckOutItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ImageUrl = i.Product.ImageUrl,
                    Price = i.Product.Price,
                    Quantity = i.Quantity
                })
                .ToList(); return new CheckoutResponseDto
            {
                Items = items,
                GrossTotal = items.Sum(i => i.Total)
            };
        }

        public async Task ExecutePaymentAsync(Guid userId, PaymentRequestDto request)
        {
            if (request.PaymentMethod != PaymentMethods.OnlinePay &&
                request.PaymentMethod != PaymentMethods.CashOnDelivery)
                throw new ArgumentException("Invalid payment method");

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("Cart is empty");

            foreach (var cartItem in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);

                if (product == null)
                    throw new InvalidOperationException("Product not found.");

                if (!product.IsActive)
                    throw new InvalidOperationException(
                        $"{product.Name} is unavailable.");

                if (product.StockQuantity < cartItem.Quantity)
                    throw new InvalidOperationException(
                        $"{product.Name} has only {product.StockQuantity} item(s) remaining.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = OrderStatuses.Pending,
                PaymentMethod = request.PaymentMethod,
                TotalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity),
                Items = cart.Items.Select(i => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            foreach (var cartItem in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);

                if (product == null)
                    continue;

                product.StockQuantity -= cartItem.Quantity;
            }

            await _productRepository.SaveChangesAsync();

            await _cartRepository.ClearCartAsync(userId);
        }
    }
}
