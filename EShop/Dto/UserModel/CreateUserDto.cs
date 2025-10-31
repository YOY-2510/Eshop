using EShop.Data;
using System.ComponentModel.DataAnnotations;

namespace EShop.Dto.UserModel
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } 
        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public IEnumerable<Guid>? RoleIds { get; set; }
    }
}
