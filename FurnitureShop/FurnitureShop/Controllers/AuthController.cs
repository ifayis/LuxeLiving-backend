using FurnitureShop.API.Common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Auth;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ApiResponse<object>.Fail(
                        ErrorMessages.ValidationFailed,
                        400,
                        ModelState.ToErrorDictionary()
                    )
                );
            }

            await _authService.RegisterAsync(request);
            return Ok(ResponseMessages.UserRegistered);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var token = await _authService.LoginAsync(request);

            if (token == null)
            {
                throw new UnauthorizedAccessException(
                    ErrorMessages.InvalidCredentials
                );
            }

            return Ok(new LoginResponseDto
            {
                Token = token
            });
        }
    }
}
