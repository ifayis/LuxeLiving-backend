namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(
            string email,
            string fullName,
            string verificationToken);

        Task SendPasswordResetEmailAsync(
            string email,
            string fullName,
            string resetToken);

        Task SendWelcomeEmailAsync(
            string email,
            string fullName);
    }
}