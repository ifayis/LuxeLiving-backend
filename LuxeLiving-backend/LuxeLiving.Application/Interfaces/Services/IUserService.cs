using LuxeLiving.Application.DTOs.Common;
using LuxeLiving.Application.DTOs.User;

namespace LuxeLiving.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<PagedResponseDto<UserResponseDto>>GetAllUsersAsync(
                int pageNumber,
                int pageSize); 
        Task<SingleUserResponseDto> GetUserByIdAsync(Guid id);
        Task BlockUserAsync(Guid userId, Guid currentAdminId);
        Task UnblockUserAsync(Guid id);
    }
}
