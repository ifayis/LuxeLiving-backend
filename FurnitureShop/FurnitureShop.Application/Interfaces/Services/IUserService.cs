using FurnitureShop.Application.DTOs.Common;
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
        Task<PagedResponseDto<UserResponseDto>>GetAllUsersAsync(
                int pageNumber,
                int pageSize); 
        Task<SingleUserResponseDto> GetUserByIdAsync(Guid id);
        Task BlockUserAsync(
            Guid userId,
            Guid currentAdminId);
        Task UnblockUserAsync(Guid id);
    }
}
