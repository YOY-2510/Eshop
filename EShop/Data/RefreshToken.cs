using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Data
{
    public class RefreshToken
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();


        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;


        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }


        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string CreatedByIp { get; set; } = string.Empty;
    }
}
