using EShop.Context;
using EShop.Data;
using EShop.Dto;
using EShop.Dto.RoleModel;
using EShop.Repositries.Interface;
using EShop.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;

namespace EShop.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        public async Task<BaseResponse<Role>> CreateRoleAsync(Role role, CancellationToken cancellationToken)
        {
            try
            { 
                Log.Information("Creating Role with Name: {RoleName}", role.Name);

                var result = new Role { Name = role.Name, Description = role.Description };
                var added = await _roleRepository.AddAsync(result, cancellationToken);

                if (!added) return BaseResponse<Role>.FailResponse("Failed to create role");
                return BaseResponse<Role>.SuccessResponse(result, "Role created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating role");
                return BaseResponse<Role>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteRoleAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Deleting Role with Id: {RoleId}", id);

                var role = await _roleRepository.GetByIdAsync(id, cancellationToken);
                if (role == null) return BaseResponse<bool>.FailResponse("Role not found");

                var deleted = await _roleRepository.DeleteAsync(role, cancellationToken);
                return deleted ? BaseResponse<bool>.SuccessResponse(true, "Deleted successfully")
                    : BaseResponse<bool>.FailResponse("Failed");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An Error occurred while deleting role");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<Role>>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching all roles");

                var roles = await _roleRepository.GetAllAsync(cancellationToken);
                return BaseResponse<IEnumerable<Role>>.SuccessResponse(roles, "Roles retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error listing roles");
                return BaseResponse<IEnumerable<Role>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<Role?>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching Role with Id: {RoleId}", id);

                var role = await _roleRepository.GetByIdAsync(id, cancellationToken);
                return role == null ? BaseResponse<Role?>.FailResponse("Role Not found")
                    : BaseResponse<Role?>.SuccessResponse(role, "Role retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting role");
                return BaseResponse<Role?>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> UpdateRoleAsync(Role role, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Updating Role with Id: {RoleId}, Name: {RoleName}", role.Id, role.Name);

                var existing = await _roleRepository.GetByIdAsync(role.Id, cancellationToken);

                if (existing == null) return BaseResponse<bool>.FailResponse("Role not found");
                existing.Name = role.Name;
                existing.Description = role.Description;

                var updated = await _roleRepository.UpdateAsync(existing, cancellationToken);
                return updated ? BaseResponse<bool>.SuccessResponse(true, "Role Updated successfully")
                    : BaseResponse<bool>.FailResponse("Failed");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating role");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}

