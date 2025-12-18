using Application.Auth.Dto.Request;
using Application.Auth.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        private void SetRefreshCookie(string refreshToken)
        {
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(14), // matcha Jwt:RefreshDays
                Path = "/"
            });
        }

        private void ClearRefreshCookie()
        {
            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });
        }

        private Guid UserId =>
         Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
             ? id
             : throw new UnauthorizedAccessException("Ingen inloggad användare.");

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 409)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> Login([FromBody] LoginDto request, CancellationToken ct)
        {
            var result = await _authService.LoginAsync(request, ct);
            SetRefreshCookie(result.RefreshToken);
            return Ok(result);
        }

        [HttpPatch("update-profile")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto request, CancellationToken ct)
        {
            var result = await _authService.UpdateProfileAsync(UserId, request, ct);
            SetRefreshCookie(result.RefreshToken);
            return Ok(result);
        }

        [HttpPut("change-password")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request, CancellationToken ct)
        {
            var result = await _authService.ChangePassword(UserId, request, ct);
            SetRefreshCookie(result.RefreshToken);
            return Ok(result);
        }

        [HttpPut("logout")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            await _authService.Logout(UserId, ct);
            ClearRefreshCookie();
            return NoContent();
        }

        [HttpPut("refreshToken")]
        [ProducesResponseType(statusCode: 200)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto request, CancellationToken ct)
        {            
                var tokens = await _jwtService.RefreshTokensAsync(request.RefreshToken, ct);
                return Ok(new
                {
                    accessToken = tokens.AccessToken,
                    refreshToken = tokens.RefreshToken
                });                  
        }
    }
}
