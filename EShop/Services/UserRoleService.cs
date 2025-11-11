using EShop.Data;
using EShop.Dto;
using EShop.Repositries;
using EShop.Repositries.Interface;
using EShop.Services.Interface;
using Serilog;

namespace EShop.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userroleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public UserRoleService(IUserRoleRepository userroleRepository, IUserRepository userRepository, IRoleRepository roleRepository)

        {
            _userroleRepository = userroleRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            try
            {// Check if user exists
                 var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                    return BaseResponse<bool>.FailResponse("User not found");

                // Check if role exists
                var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);
                if (role == null)
                    return BaseResponse<bool>.FailResponse("Role not found");

                // Check if this user already has that role
                var existing = await _userroleRepository.GetByUserIdAndRoleIdAsync(userId, roleId, cancellationToken);
                if (existing != null)
                    return BaseResponse<bool>.FailResponse("User already has this role");

                var added = await _userroleRepository.AddAsync(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                }, cancellationToken);

                return added
                    ? BaseResponse<bool>.SuccessResponse(true, "Role assigned successfully")
                    : BaseResponse<bool>.FailResponse("Failed to assign role");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error assigning role");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        //public async Task<BaseResponse<bool>> AssignRoleToUserAsync(UserRole userRole, CancellationToken cancellationToken)
        //{
        //    var result = await _userroleRepository.AddAsync(userRole, cancellationToken);

        //    return result
        //        ? BaseResponse<bool>.SuccessResponse(true, "Role assigned")
        //        : BaseResponse<bool>.FailResponse("Failed to assign role");
        //}

        public async Task<BaseResponse<bool>> DeleteUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Deleting UserRole {UserId} {RoleId}", userId, roleId);

                var deleted = await _userroleRepository.DeleteByIdsAsync(userId, roleId, cancellationToken);
                return deleted ? BaseResponse<bool>.SuccessResponse(true, "User-Role Deleted successfully")
                    : BaseResponse<bool>.FailResponse("Not found");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting User-Role");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<Role>>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching roles for UserId: {UserId}", userId);

                var roles = await _userroleRepository.GetRolesByUserIdAsync(userId, cancellationToken);
                return BaseResponse<IEnumerable<Role>>.SuccessResponse(roles, "Roles fetched successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching roles for user");
                return BaseResponse<IEnumerable<Role>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<User>>> GetUsersByRoleAsync(Guid roleId, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching users for RoleId: {RoleId}", roleId);

                var users = await _userroleRepository.GetUsersByRoleIdAsync(roleId, cancellationToken);
                return BaseResponse<IEnumerable<User>>.SuccessResponse(users, "Users fetched successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching users for role");
                return BaseResponse<IEnumerable<User>>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}
