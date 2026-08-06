using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Application.DTOs.ShippingAddress
{
    public class ShippingAddressResponseDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PinCode { get; set; } = string.Empty;

        public AddressType AddressType { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}