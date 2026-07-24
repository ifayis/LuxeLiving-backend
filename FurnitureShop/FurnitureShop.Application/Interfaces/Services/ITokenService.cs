using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        string GenerateEmailVerificationToken();

        string GeneratePasswordResetToken();
    }
}