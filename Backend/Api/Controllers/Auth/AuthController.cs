using Application.Auth.Dto.Request;
using Application.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

        [HttpPost("register")]
        [ProducesResponseType(statusCode:201)]
        [ProducesResponseType(statusCode:409)]
        [ProducesResponseType(statusCode:400)]
        public async Task<IActionResult> Register([FromBody]RegisterDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), result);
        }

        [HttpPost("login")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        [ProducesResponseType(statusCode:409)]
        public async Task<IActionResult> Login([FromBody] LoginDto request, CancellationToken ct)
        {
            var result = await _authService.LoginAsync(request, ct);
            SetRefreshCookie(result.RefreshToken);
            return Ok(result);
        }
    }
}
