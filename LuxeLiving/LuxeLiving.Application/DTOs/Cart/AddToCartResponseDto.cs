namespace FurnitureShop.Application.DTOs.Cart
{
    public class AddToCartResponseDto
    {
        public Guid CartId { get; set; }

        public AddedCartProductDto Product { get; set; } = null!;

        public int CartItemCount { get; set; }

        public decimal CartTotal { get; set; }
    }
}