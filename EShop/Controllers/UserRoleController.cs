using EShop.Dto;
using EShop.Dto.UserRoleModel;
using EShop.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService userRoleService)
            => _userRoleService = userRoleService;

        [Authorize(Roles = "Admin")]
        [HttpPost("Assign-role")]
        public async Task<IActionResult> Assign([FromBody] AssignRoleDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
                return BadRequest(BaseResponse<bool>.FailResponse("Invalid input"));
            var result = await _userRoleService.AssignRoleToUserAsync(dto.UserId, dto.RoleId, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-role")]
        public async Task<IActionResult> DeleteRole([FromBody] AssignRoleDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
                return BadRequest(BaseResponse<bool>.FailResponse("Invalid input"));
            var result = await _userRoleService.DeleteUserRoleAsync(dto.UserId, dto.RoleId, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("Get-roles-by-user/{userId:guid}/roles")]
        public async Task<IActionResult> GetRoles(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _userRoleService.GetUserRolesAsync(userId, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("Get-users-by-role/{roleId:guid}/users")]
        public async Task<IActionResult> GetUsers(Guid roleId, CancellationToken cancellationToken)
        {
            var result = await _userRoleService.GetUsersByRoleAsync(roleId, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }
    }
}
