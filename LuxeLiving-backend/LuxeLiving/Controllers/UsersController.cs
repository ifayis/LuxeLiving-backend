using LuxeLiving.Application.common;
using LuxeLiving.Application.Common;
using LuxeLiving.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxeLiving.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(
            IUserService userService)
        {
            _userService = userService;
        }


        private Guid GetCurrentAdminId()
        {
            var userId = User.FindFirst("UID")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException();
            }

            return Guid.Parse(userId);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var users = await _userService
                .GetAllUsersAsync(
                    pageNumber,
                    pageSize
                );

            return Ok(users);
        }
        

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var user = await _userService
                 .GetUserByIdAsync(userId);

            return Ok(user);
        }


        [HttpPatch("{userId:guid}/block")]
        public async Task<IActionResult> Block(
            Guid userId)
        {
            await _userService.BlockUserAsync(
                userId,
                GetCurrentAdminId()
            );

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    "User blocked successfully."
                )
            );
        }


        [HttpPatch("{userId:guid}/unblock")]
        public async Task<IActionResult> Unblock(Guid userId)
        {
            await _userService.UnblockUserAsync(userId);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    "User unblocked successfully."
                )
            );
        }
    }
}