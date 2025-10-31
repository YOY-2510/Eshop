using EShop.Dto;
using EShop.Data;
using EShop.Dto.AuthModel;

namespace EShop.Services.Interface
{
    public interface IAuthService
    {
        Task<BaseResponse<TokenResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<BaseResponse<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
    }
}
