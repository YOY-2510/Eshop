using EShop.Data;
using EShop.Dto;
using EShop.Dto.ProductModel;
using EShop.Dto.RoleModel;
using EShop.Dto.UserModel;
using EShop.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) 
            => _roleService = roleService;

        [Authorize(Roles = "Admin")]
        [HttpPost("create-roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto, CancellationToken cancellationtoken)
        {
            var role = new Role { Name = dto.Name, Description = dto.Description };
            var result = await _roleService.CreateRoleAsync(role, cancellationtoken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("Get-all-roles")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationtoken)
        {
            var result = await _roleService.GetAllRolesAsync(cancellationtoken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-Role/{id:guid}")]
        public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationtoken)
        {
            var result = await _roleService.DeleteRoleAsync(id, cancellationtoken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }
    }
}
