using EShop.Context;
using EShop.Data;
using EShop.Dto.UserModel;
using EShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;


namespace EShop.Repositries
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRoleRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> AddAsync(UserRole userRole, CancellationToken cancellationToken)
        {
            await _dbContext.UserRoles.AddAsync(userRole, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<UserRole?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
        }

        public async Task<bool> DeleteByIdsAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            var ur = await _dbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
            if (ur == null) 
                return false;
            _dbContext.UserRoles.Remove(ur);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async  Task<IEnumerable<UserRole>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.UserRoles.ToListAsync(cancellationToken);
        }

        public async Task<UserRole?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserRoles
               .FirstOrDefaultAsync(ur => ur.UserId == userId, cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {

            return await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role!)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId, CancellationToken cancellationToken)
        {

            return await _dbContext.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .Include(ur => ur.User)
                .Select(ur => ur.User!)
                .ToListAsync(cancellationToken);
        }

        //public async Task<UserRole?> GetUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        //{
        //    return await _dbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);
        //}

        //public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId, CancellationToken cancellationToken)
        //{
        //    return await _dbContext.UserRoles
        //        .Where(ur => ur.RoleId == roleId)
        //        .Include(ur => ur.User)
        //        .Select(ur => ur.User!)
        //        .ToListAsync(cancellationToken);
        //}
    }
}
