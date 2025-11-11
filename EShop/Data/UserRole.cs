using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace EShop.Data
{
    [Table("UserRoles")]
    public class UserRole 
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public User? User { get; set; } 

        public Guid RoleId { get; set; }
        public Role? Role { get; set; } 
    }
}
