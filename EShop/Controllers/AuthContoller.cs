using EShop.Dto;
using EShop.Dto.AuthModel;
using EShop.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EShop.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(BaseResponse<string>.FailResponse("Invalid Input data."));

            var response = await _authService.RegisterAsync(request, cancellationToken);

            if (!response.Status)
            {
                Log.Warning("Registration failed: {Message}", response.Message);
                return BadRequest(response);
            }

            Log.Information("User registered: {Email}", request.Email);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(BaseResponse<string>.FailResponse("Invalid input data"));

            var response = await _authService.LoginAsync(request, cancellationToken);

            if (!response.Status)
            {
                Log.Warning("Login failed for user {Email}: {Message}", request.Email, response.Message);
                return Unauthorized(response);
            }

            Log.Information("User logged in: {Email}", request.Email);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(BaseResponse<string>.FailResponse("Refresh token is required."));

            var response = await _authService.RefreshTokenAsync(request, cancellationToken);

            if (!response.Status)
            {
                Log.Warning("Refresh token failed: {Message}", response.Message);
                return Unauthorized(response);
            }

            Log.Information("Refresh token successful for token: {Token}", request.RefreshToken);
            return Ok(response);
        }
    }
}
