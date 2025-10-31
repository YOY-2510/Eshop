using EShop.Data;
using EShop.Dto;

namespace EShop.Services.Interface
{
    public interface IRoleService
    {
        Task<BaseResponse<Role>> CreateRoleAsync(Role role, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<Role>>> GetAllRolesAsync(CancellationToken cancellationToken);
        Task<BaseResponse<Role?>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateRoleAsync(Role role, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteRoleAsync(Guid id, CancellationToken cancellationToken);
    }
}
