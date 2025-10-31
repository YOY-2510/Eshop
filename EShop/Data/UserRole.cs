using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Data
{
    [Table("UserRoles")]
    public class UserRole 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public User? User { get; set; } 

        public Guid RoleId { get; set; }
        public Role? Role { get; set; } 
    }
}
