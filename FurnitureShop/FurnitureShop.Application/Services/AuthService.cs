using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Auth;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using Microsoft.Extensions.Configuration;

namespace FurnitureShop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task RegisterAsync(RegisterRequestDto request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
                throw new InvalidOperationException(ErrorMessages.UserAlreadyExists);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var verificationToken =
                _tokenService.GenerateEmailVerificationToken();

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName.Trim(),
                Email = email,
                PasswordHash = passwordHash,
                Role = Roles.User,
                IsBlocked = false,
                IsEmailVerified = false,
                EmailVerificationToken = verificationToken,
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            await _userRepository.AddAsync(user);

            try
            {
                await _emailService.SendEmailVerificationAsync(
                    user.Email,
                    user.FullName,
                    verificationToken);
            }
            catch
            {
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                throw new UnauthorizedAccessException(ErrorMessages.InvalidCredentials);

            if (user.IsBlocked)
                throw new UnauthorizedAccessException(ErrorMessages.UserBlocked);

            if (user.LockoutEnd.HasValue &&
                user.LockoutEnd > DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(
                    ErrorMessages.AccountLocked);
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                    user.FailedLoginAttempts = 0;
                }

                await _userRepository.SaveChangesAsync();

                throw new UnauthorizedAccessException(
                    ErrorMessages.InvalidCredentials);
            }

            if (!user.IsEmailVerified)
                throw new UnauthorizedAccessException(
                    ErrorMessages.EmailNotVerified);

            var accessToken = _tokenService.GenerateAccessToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenDays =
                _configuration.GetValue<int>("JwtSettings:RefreshTokenDays");

            var accessTokenMinutes =
                _configuration.GetValue<int>("JwtSettings:AccessTokenMinutes");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(refreshTokenDays);

            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            user.LastLoginAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenMinutes)
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException(ErrorMessages.InvalidRefreshToken);

            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null)
                throw new UnauthorizedAccessException(ErrorMessages.InvalidRefreshToken);

            if (user.IsBlocked)
                throw new UnauthorizedAccessException(ErrorMessages.UserBlocked);

            if (!user.RefreshTokenExpiryTime.HasValue ||
                user.RefreshTokenExpiryTime.Value <= DateTime.UtcNow)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                await _userRepository.SaveChangesAsync();

                throw new UnauthorizedAccessException(ErrorMessages.RefreshTokenExpired);
            }

            var refreshTokenDays =
                _configuration.GetValue<int>("JwtSettings:RefreshTokenDays");

            var accessTokenMinutes =
                _configuration.GetValue<int>("JwtSettings:AccessTokenMinutes");

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(refreshTokenDays);

            await _userRepository.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenMinutes)
            };
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException(ErrorMessages.UserNotFound);

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.SaveChangesAsync();
        }

        public async Task<CurrentUserResponseDto> GetCurrentUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException(ErrorMessages.UserNotFound);

            return new CurrentUserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsBlocked = user.IsBlocked,
                IsEmailVerified = user.IsEmailVerified
            };
        }

        public async Task ChangePasswordAsync(
            Guid userId,
            ChangePasswordRequestDto request)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException(ErrorMessages.UserNotFound);

            if (!BCrypt.Net.BCrypt.Verify(
                request.CurrentPassword,
                user.PasswordHash))
            {
                throw new UnauthorizedAccessException(
                    ErrorMessages.CurrentPasswordIncorrect);
            }

            if (BCrypt.Net.BCrypt.Verify(
                request.NewPassword,
                user.PasswordHash))
            {
                throw new InvalidOperationException(
                    ErrorMessages.PasswordCannotBeSame);
            }

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.UpdatedAt = DateTime.UtcNow;
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.UpdateAsync(user);

            await _userRepository.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(
            ForgotPasswordRequestDto request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return;

            var token = _tokenService.GeneratePasswordResetToken();

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await _emailService.SendPasswordResetEmailAsync(
                user.Email,
                user.FullName,
                token);
        }

        public async Task ResetPasswordAsync(
            ResetPasswordRequestDto request)
        {
            var user = await _userRepository
                .GetByPasswordResetTokenAsync(request.Token);

            if (user == null)
                throw new UnauthorizedAccessException(
                    ErrorMessages.InvalidResetToken);

            if (!user.PasswordResetTokenExpiry.HasValue ||
                user.PasswordResetTokenExpiry.Value <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(
                    ErrorMessages.ResetTokenExpired);
            }

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task VerifyEmailAsync(
            VerifyEmailRequestDto request)
        {
            var user = await _userRepository
                .GetByEmailVerificationTokenAsync(request.Token);

            if (user == null)
                throw new UnauthorizedAccessException(
                    ErrorMessages.InvalidVerificationToken);

            if (!user.EmailVerificationTokenExpiry.HasValue ||
                user.EmailVerificationTokenExpiry.Value <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(
                    ErrorMessages.VerificationTokenExpired);
            }

            if (user.IsEmailVerified)
                return;

            user.IsEmailVerified = true;
            user.LastLoginAt = DateTime.UtcNow;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await _emailService.SendWelcomeEmailAsync(
                user.Email,
                user.FullName);
        }

        public async Task ResendVerificationEmailAsync(
            ResendVerificationRequestDto request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return;

            if (user.IsEmailVerified)
                return;

            var token = _tokenService.GenerateEmailVerificationToken();

            user.EmailVerificationToken = token;
            user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            await _emailService.SendEmailVerificationAsync(
                user.Email,
                user.FullName,
                token);
        }
    }
}
