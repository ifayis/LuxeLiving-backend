using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Auth;
using FurnitureShop.Application.Interfaces;
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
            await _authService.RegisterAsync(request);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var token = await _authService.LoginAsync(request);

            if (token == null)
            {
                return Unauthorized(
                    ApiResponse<string>.Fail("Invalid credentials", 401)
                );
            }

            var response = ApiResponse<LoginResponseDto>.Success(
                new LoginResponseDto { Token = token },
                "Login successful"
            );

            return Ok(response);
        }

    }
}
