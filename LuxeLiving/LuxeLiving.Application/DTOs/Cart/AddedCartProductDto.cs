namespace FurnitureShop.Application.DTOs.Cart
{
    public class AddedCartProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}