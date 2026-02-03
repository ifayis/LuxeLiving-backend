using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.User;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("UID")!.Value);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("Individual/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("Block/{id:guid}")]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            var success = await _userService.BlockUserAsync(id);
            if (!success) return NotFound();

            return Ok(ApiResponse<object>.Success(null, "User blocked"));
        }

        [HttpPut("Unblock/{id:guid}")]
        public async Task<IActionResult> UnblockUser(Guid id)
        {
            var success = await _userService.UnblockUserAsync(id);
            if (!success) return NotFound();

            return Ok(ApiResponse<object>.Success(null, "User unblocked"));
        }
    }
}
