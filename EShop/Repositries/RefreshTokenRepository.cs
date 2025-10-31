using EShop.Context;
using EShop.Data;
using EShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositries
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(token, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _dbContext.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token, cancellationToken);        
        }

        public async Task<bool> DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var tokens = _dbContext.RefreshTokens.Where(r => r.UserId == userId);
            _dbContext.RefreshTokens.RemoveRange(tokens);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UpdateAsync(RefreshToken token, CancellationToken cancellationToken)
        {
            _dbContext.RefreshTokens.Update(token);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
