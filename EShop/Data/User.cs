using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required,MinLength(3), MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required,MinLength(6),MaxLength(20)]
        public string PassWord { get; set; } = string.Empty;

        [Required,MinLength(10),MaxLength(11)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;

        public Gender? Gender { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}