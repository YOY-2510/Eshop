using EShop.Data;

namespace EShop.Repositries.Interface
{
    public interface IRoleRepository
    {
        Task<bool> AddAsync(Role role, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Role role, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Role role, CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken);
    }
}
