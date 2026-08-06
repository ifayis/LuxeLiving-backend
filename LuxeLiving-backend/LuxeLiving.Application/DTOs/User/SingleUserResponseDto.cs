namespace LuxeLiving.Application.DTOs.User
{
    public class SingleUserResponseDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public Guid? CartId { get; set; }

        public Guid? WishlistId { get; set; }

        public bool IsBlocked { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public bool IsEmailVerified { get; set; }
    }
}
