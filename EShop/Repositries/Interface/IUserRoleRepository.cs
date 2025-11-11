using EShop.Data;

namespace EShop.Repositries.Interface
{
    public interface IUserRoleRepository
    {
        Task<bool> AddAsync(UserRole userRole, CancellationToken cancellationToken);
        Task<IEnumerable<UserRole>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> DeleteByIdsAsync(Guid userId,Guid roleId, CancellationToken cancellationToken);
        Task<UserRole?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId, CancellationToken cancellationToken);
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        //Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        //Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId, CancellationToken cancellationToken);
    }
}
