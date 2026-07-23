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

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName.Trim(),
                Email = request.Email.ToLower().Trim(),
                PasswordHash = passwordHash,
                Role = Roles.User
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var email = request.Email.Trim().ToUpperInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                throw new UnauthorizedAccessException(ErrorMessages.InvalidCredentials);

            if (user.IsBlocked)
                throw new UnauthorizedAccessException("Your account is blocked by admin");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException(ErrorMessages.InvalidCredentials);

            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            if (token == null)
            {
                throw new UnauthorizedAccessException(
                    ErrorMessages.InvalidCredentials
                );
            }


            user.RefreshToken = refreshToken;
            var refreshDays = _configuration.GetValue<int>("JwtSettings:RefreshTokenDays");

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshDays);

            await _userRepository.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("JwtSettings:AccessTokenMinutes"))
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired");

            if (user.IsBlocked)
                throw new UnauthorizedAccessException("User is blocked");

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            await _userRepository.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("JwtSettings:AccessTokenMinutes"))
            };
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.SaveChangesAsync();
        }
    }
}
