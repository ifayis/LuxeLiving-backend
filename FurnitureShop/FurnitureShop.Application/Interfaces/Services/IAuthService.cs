using FurnitureShop.Application.DTOs.Auth;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);

        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);

        Task LogoutAsync(Guid userId);

        Task<CurrentUserResponseDto> GetCurrentUserAsync(Guid userId);

        Task ChangePasswordAsync(
            Guid userId,
            ChangePasswordRequestDto request);

        Task ForgotPasswordAsync(
            ForgotPasswordRequestDto request);

        Task ResetPasswordAsync(
            ResetPasswordRequestDto request);

        Task VerifyEmailAsync(
            VerifyEmailRequestDto request);

        Task ResendVerificationEmailAsync(
            ResendVerificationRequestDto request);
    }
}