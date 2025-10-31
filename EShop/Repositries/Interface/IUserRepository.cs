using EShop.Data;
using EShop.Dto;
using EShop.Dto.ProductModel;
using EShop.Dto.UserModel;

namespace EShop.Repositries.Interface
{
    public interface IUserRepository
    {
        Task<bool> AddAsync(User user, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(User user, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    }
}
