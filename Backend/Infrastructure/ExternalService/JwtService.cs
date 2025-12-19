using Application.Auth.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly FoodyDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public JwtService(IConfiguration configuration, FoodyDbContext context,Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }
        public string GenerateToken(User user, IList<string> roles)
        {
            var jwtId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),

                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:ExpireInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task RevokeAllAsync(User user, CancellationToken ct)
        {
            await _context.Entry(user)
                .Collection(u => u.RefreshTokens)
                .LoadAsync(ct);

            var now = DateTime.UtcNow;

            foreach (var token in user.RefreshTokens.Where(t => !t.IsRevoked))
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync(ct);
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshTokenValue, CancellationToken ct)
        {
            var hashed = Sha256(refreshTokenValue);
            // 1. Lookup the token in DB
            var refreshToken = await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == hashed, ct);

            if (refreshToken == null)
                throw new SecurityTokenException("Refresh token not found");

            var user = refreshToken.User;

            // 2. Validate the token
            if (refreshToken.IsRevoked || refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Refresh token invalid or expired");
            }

            // 3. Issue new tokens
            return await ReissueTokensAsync(refreshToken.User, ct);
        }

        public string GenerateSecureToken()
        {
            var randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string Sha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        public async Task<(string AccessToken, string RefreshToken)> ReissueTokensAsync(User user, CancellationToken ct)
        {
            await RevokeAllAsync(user, ct);

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = GenerateToken(user, roles);
            var refreshToken = GenerateSecureToken();

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = Sha256(refreshToken),
                JwtId = Sha256(accessToken),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAtUtc = DateTime.UtcNow
            });

            await _userManager.UpdateAsync(user);

            return (accessToken, refreshToken);
        }
    }
}
