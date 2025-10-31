using EShop.Data;
using EShop.Dto.RoleModel;

namespace EShop.Dto.UserModel
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } 
        public Gender? Gender { get; set; }
        public string? Address { get; set; } 
        public IEnumerable<RoleDto>? Roles { get; set; }
    }
}
