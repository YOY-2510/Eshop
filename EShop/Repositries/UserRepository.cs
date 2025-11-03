using Azure.Core;
using EShop.Context;
using EShop.Data;
using EShop.Dto.UserModel;
using EShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EShop.Repositries
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;
      
        public async Task<bool> AddAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(user, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
        }

        public async Task<bool> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Remove(user);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
