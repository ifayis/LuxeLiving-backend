namespace LuxeLiving.Application.DTOs.User
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsBlocked { get; set; }
    }
}
