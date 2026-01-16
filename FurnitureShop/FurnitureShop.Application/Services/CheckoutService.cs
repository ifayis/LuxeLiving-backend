using FurnitureShop.Application.DTOs.Checkout;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;

        public CheckoutService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        public async Task<CheckoutResponseDto> GetCheckoutAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
                return new CheckoutResponseDto();

            var items = cart.Items.Select(i => new CheckOutItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ImageUrl = i.Product.ImageUrl,
                Price = i.Product.Price,
                Quantity = i.Quantity
            }).ToList();

            return new CheckoutResponseDto
            {
                Items = items,
                GrossTotal = items.Sum(i => i.Total)
            };
        }

        public async Task ExecutePaymentAsync(Guid userId, PaymentRequestDto request)
        {
            if (request.PaymentMethod != "Online Pay" &&
                request.PaymentMethod != "Cash On Delivery")
                throw new ArgumentException("Invalid payment method");

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("Cart is empty");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = request.PaymentMethod == "Online Pay" ? "Paid" : "Pending",
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

            _cartRepository.Clear(cart);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
