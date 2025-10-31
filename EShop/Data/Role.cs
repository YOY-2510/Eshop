using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Data
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public string? Description { get; set; } 
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
