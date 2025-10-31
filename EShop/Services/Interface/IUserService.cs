using EShop.Data;
using EShop.Dto;
using EShop.Dto.ProductModel;
using EShop.Dto.UserModel;

namespace EShop.Services.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<bool>> AddUserAsync(User user, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<BaseResponse<User?>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken);
        Task<BaseResponse<bool>> AssignRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteUserAsync(User user, CancellationToken cancellationToken);
    }
}
