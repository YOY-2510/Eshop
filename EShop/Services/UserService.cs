using Azure.Core;
using EShop.Context;
using EShop.Data;
using EShop.Dto;
using EShop.Dto.UserModel;
using EShop.Repositries.Interface;
using EShop.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Threading;

namespace EShop.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        public async Task<BaseResponse<bool>> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Creating User with details {User}", user);

                if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.PassWord))
                    return BaseResponse<bool>.FailResponse("UserName, Email and Password are required.");

                Log.Information("Creating user {UserName}", user.UserName);
                var result = await _userRepository.AddAsync(user, cancellationToken);
                return result ? BaseResponse<bool>.SuccessResponse(true, "User created successfully.")
                    : BaseResponse<bool>.FailResponse("Failed to create user.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An Error occurred while creating user");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> AssignRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Assigning Role to User {UserId}, {RoleId}", userId, roleId);

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null) return BaseResponse<bool>.FailResponse("User not found");

                var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);
                if (role == null) return BaseResponse<bool>.FailResponse("Role not found");

                var existing = await _userRoleRepository.GetUserRoleAsync(userId, roleId, cancellationToken);
                if (existing != null) return BaseResponse<bool>.FailResponse("User already has this role");

                var ur = new UserRole { UserId = userId, RoleId = roleId };
                var result = await _userRoleRepository.AddAsync(ur, cancellationToken);
                return result ? BaseResponse<bool>.SuccessResponse(true, "Role assigned successfully") 
                    : BaseResponse<bool>.FailResponse("Failed to assign role");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error assigning role");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteUserAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Deleting User {User}", user);

                var result = await _userRepository.DeleteAsync(user, cancellationToken);
                return result ? BaseResponse<bool>.SuccessResponse(true, "Deleted successfully") 
                    : BaseResponse<bool>.FailResponse("Failed to delete");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting user");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching all users");

                var users = await _userRepository.GetAllAsync(cancellationToken);
                return BaseResponse<IEnumerable<User>>.SuccessResponse(users, "Users retrieved");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting users");
                return BaseResponse<IEnumerable<User>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<User?>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching User {User}", id);

                var user = await _userRepository.GetByIdAsync(id, cancellationToken);
                return user == null ? BaseResponse<User?>.FailResponse("User not found")
                    : BaseResponse<User?>.SuccessResponse(user, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting user by id");
                return BaseResponse<User?>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Updating User {User}", user);

                var existing = await _userRepository.GetByIdAsync(user.Id, cancellationToken);
                if (existing == null) return BaseResponse<bool>.FailResponse("User not found");

                existing.UserName = user.UserName;
                existing.Email = user.Email;
                existing.PhoneNumber = user.PhoneNumber;
                existing.Gender = user.Gender;
                existing.Address = user.Address;

                var result = await _userRepository.UpdateAsync(existing, cancellationToken);
                return result ? BaseResponse<bool>.SuccessResponse(true, "Updated user successfully")
                    : BaseResponse<bool>.FailResponse("Failed to update");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating user");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}
