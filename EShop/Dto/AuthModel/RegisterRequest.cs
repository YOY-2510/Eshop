using System.ComponentModel.DataAnnotations;

namespace EShop.Dto.AuthModel
{
    public class RegisterRequest
    {
        [Required,MinLength(3),MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email {  get; set; } = string.Empty;

        [Required, MinLength(6),MaxLength(20)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string RoleName {  get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required,MinLength(6),MaxLength(20)]
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
