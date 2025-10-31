using EShop.Context;
using EShop.Data;
using EShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositries
{
    public class RoleRepository: IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> AddAsync(Role role, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(role, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            _dbContext.Roles.Remove(role);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.ToListAsync(cancellationToken);
        }

        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            _dbContext.Roles.Update(role);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
