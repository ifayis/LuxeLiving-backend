using System.ComponentModel.DataAnnotations;
using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Domain.Entities
{
    public class ShippingAddress
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = "India";

        public string PinCode { get; set; } = string.Empty;

        public AddressType AddressType { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
            = DateTime.UtcNow;

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}