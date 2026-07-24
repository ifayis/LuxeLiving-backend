namespace FurnitureShop.Application.DTOs.Auth
{
    public class CurrentUserResponseDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsBlocked { get; set; }

        public bool IsEmailVerified { get; set; }
    }
}