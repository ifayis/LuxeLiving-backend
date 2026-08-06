using LuxeLiving.Domain.Enitities;

namespace LuxeLiving.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        string GenerateEmailVerificationToken();
        string GeneratePasswordResetToken();
    }
}