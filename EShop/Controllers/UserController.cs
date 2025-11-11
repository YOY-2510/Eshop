using EShop.Data;
using EShop.Dto.ProductModel;
using EShop.Dto;
using EShop.Dto.UserModel;
using EShop.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EShop.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
            => _userService = userService;

        [HttpPost("Create user")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
        {
            var result = await _userService.AddUserAsync(dto, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("Get-all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllAsync(cancellationToken);
            return result.Status ? Ok(result) 
                : BadRequest(result);
        }

        [HttpGet("Get-user/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _userService.GetByIdAsync(id, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] User user, CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateUserAsync(user, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-user/{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            var fetch = await _userService.GetByIdAsync(id, cancellationToken);
            if (!fetch.Status || fetch.Data == null)
                return NotFound(fetch);
            var result = await _userService.DeleteUserAsync(fetch.Data, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }

        [HttpPost("Assign-role/{userId:guid}/{roleId:guid}")]
        public async Task<IActionResult> AssignRole(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            var result = await _userService.AssignRoleAsync(userId, roleId, cancellationToken);
            return result.Status ? Ok(result)
                : BadRequest(result);
        }
    }
}
