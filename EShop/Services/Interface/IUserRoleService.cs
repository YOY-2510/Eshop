using EShop.Data;
using EShop.Dto;

namespace EShop.Services.Interface
{
    public interface IUserRoleService
    {
        Task<BaseResponse<bool>> AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<Role>>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<User>>> GetUsersByRoleAsync(Guid roleId, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
    }
}
