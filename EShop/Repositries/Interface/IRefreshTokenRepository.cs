using EShop.Data;

namespace EShop.Repositries.Interface
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token, CancellationToken cancellationToken);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
        Task<bool> DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(RefreshToken token, CancellationToken cancellationToken);

    }
}
