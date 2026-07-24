using FurnitureShop.API.Common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Auth;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        StatusCodes.Status400BadRequest,
                        ModelState.ToErrorDictionary()));
            }

            await _authService.RegisterAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                ResponseMessages.UserRegistered);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request)
        {
            return Ok(await _authService.LoginAsync(request));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequestDto request)
        {
            return Ok(await _authService.RefreshTokenAsync(request.RefreshToken));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = GetCurrentUserId();

            await _authService.LogoutAsync(userId);

            return Ok(ResponseMessages.LogoutSuccessful);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = GetCurrentUserId();

            return Ok(await _authService.GetCurrentUserAsync(userId));
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordRequestDto request)
        {
            var userId = GetCurrentUserId();

            await _authService.ChangePasswordAsync(userId, request);

            return Ok(ResponseMessages.PasswordChangedSuccessfully);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequestDto request)
        {
            await _authService.ForgotPasswordAsync(request);

            return Ok(ResponseMessages.PasswordResetEmailSent);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordRequestDto request)
        {
            await _authService.ResetPasswordAsync(request);

            return Ok(ResponseMessages.PasswordResetSuccessful);
        }

        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailRequestDto request)
        {
            await _authService.VerifyEmailAsync(request);

            return Ok(ResponseMessages.EmailVerifiedSuccessfully);
        }

        [AllowAnonymous]
        [HttpPost("resend-verification-email")]
        public async Task<IActionResult> ResendVerificationEmail(
            [FromBody] ResendVerificationRequestDto request)
        {
            await _authService.ResendVerificationEmailAsync(request);

            return Ok(ResponseMessages.VerificationEmailSent);
        }

        private Guid GetCurrentUserId()
        {
            var userId = User.FindFirstValue("UID");

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException(ErrorMessages.InvalidToken);

            return Guid.Parse(userId);
        }
    }
}