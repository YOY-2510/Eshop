using EShop.Data;
using EShop.Dto;
using EShop.Dto.AuthModel;
using EShop.Repositries;
using EShop.Repositries.Interface;
using EShop.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userroleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository,
                           IRoleRepository roleRepository,
                           IUserRoleRepository userroleRepository,
                           IRefreshTokenRepository refreshTokenRepository,
                           IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userroleRepository = userroleRepository;
            _refreshTokenRepository =  refreshTokenRepository;
            _configuration = configuration;
        }

        public async Task<BaseResponse<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllAsync(cancellationToken);
                var user = users.FirstOrDefault(u => u.Email == request.Email && u.PassWord == request.Password);

                if (user == null)
                    return BaseResponse<TokenResponse>.FailResponse("Invalid credentials.");

                var userRoles = await _userroleRepository.GetAllAsync(cancellationToken);
                var ur = userRoles.FirstOrDefault(x => x.UserId == x.Id);

                var roleName = "User";
                if (ur != null)
                {
                    var role = await _roleRepository.GetByIdAsync(ur.RoleId, cancellationToken);
                    if (role != null) roleName = role.Name;
                }

                var accessToken = GenerateJwtToken(user, roleName);
                var refreshToken = GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken()
                {
                    Token = refreshToken,
                    Expires = DateTime.UtcNow.AddDays(7),
                    UserId = user.Id,
                    CreatedByIp = "127.0.0.1"
                };

                await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

                return BaseResponse<TokenResponse>.SuccessResponse(new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                }, "Login Successful.");
            }
            catch (Exception ex)
            {
                return BaseResponse<TokenResponse>.FailResponse($"Error:{ex.Message}");
            }
        }


        public async Task<BaseResponse<TokenResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = (await _userRepository.GetAllAsync(cancellationToken))
                    .FirstOrDefault(u => u.Email == request.Email);

                if (existingUser == null)
                    return BaseResponse<TokenResponse>.FailResponse("Email already registered.");

                var newUser = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    PassWord = request.Password.Trim()
                };

                var userSaved = await _userRepository.AddAsync(newUser, cancellationToken);
                if (!userSaved)
                    return BaseResponse<TokenResponse>.FailResponse("Failed to save user.");

                var role = (await _roleRepository.GetAllAsync(cancellationToken))
                    .FirstOrDefault(r => r.Name.ToLower() == request.RoleName.ToLower());

                if (role == null)
                    return BaseResponse<TokenResponse>.FailResponse($"Role '{request.RoleName}' not found.");

                var userRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = role.Id
                };
                await _userroleRepository.AddAsync(userRole, cancellationToken);

                var accesstoken = GenerateJwtToken(newUser, role.Name);
                var refreshToken = GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshToken,
                    Expires = DateTime.UtcNow.AddDays(7),
                    UserId = newUser.Id,
                    CreatedByIp = "127.0.0.1"
                };

                await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

                return BaseResponse<TokenResponse>.SuccessResponse(new TokenResponse
                {
                    AccessToken = accesstoken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                }, "User registered successfully.");
            }
            catch (Exception ex)
            {
                return BaseResponse<TokenResponse>.FailResponse($"Error:{ex.Message}");
            }
        }

        public async Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
                if (storedToken == null || storedToken.Expires < DateTime.UtcNow)
                    return BaseResponse<TokenResponse>.FailResponse("Invalid or expired refresh token.");

                var user = await _userRepository.GetByIdAsync(storedToken.UserId, cancellationToken);
                if (user == null)
                    return BaseResponse<TokenResponse>.FailResponse("User not found.");

                var userRoles = await _userroleRepository.GetAllAsync(cancellationToken);
                var ur = userRoles.FirstOrDefault(x => x.UserId == user.Id);
                var roleName = "User";

                if (ur != null)
                {
                    var role = await _roleRepository.GetByIdAsync(ur.RoleId, cancellationToken);
                    if (role != null) roleName = role.Name;
                }

                var newAccessToken = GenerateJwtToken(user, roleName);
                var newRefreshToken = GenerateRefreshToken();

                storedToken.Token = newRefreshToken;
                storedToken.Expires = DateTime.UtcNow.AddDays(7);
                await _refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);

                return BaseResponse<TokenResponse>.SuccessResponse(new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]))
                }, "Token refreshed successfully.");
            }
            catch (Exception ex)
            {
                return BaseResponse<TokenResponse>.FailResponse($"Error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
