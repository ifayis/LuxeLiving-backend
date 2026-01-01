using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using FurnitureShop.API.Common;
using FurnitureShop.Application.Interfaces.Services;

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
                return BadRequest(ModelState.ToErrorDictionary());
            }

            await _authService.RegisterAsync(request);

            return Ok("User registered successfully");
            ;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var token = await _authService.LoginAsync(request);

            if (token == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return Ok(new LoginResponseDto { Token = token });
        }
    }
}
