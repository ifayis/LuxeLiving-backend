using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<User?> GetByPasswordResetTokenAsync(string resetToken);
        Task<User?> GetByEmailVerificationTokenAsync(string verificationToken);
        Task<List<User>> GetAllAsync();
        Task<int> CountAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();
    }
}