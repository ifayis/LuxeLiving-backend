using FurnitureShop.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<SingleUserResponseDto> GetUserByIdAsync(Guid id);
        Task<bool> BlockUserAsync(
            Guid userId,
            Guid currentAdminId);
        Task<bool> UnblockUserAsync(Guid id);
    }
}
