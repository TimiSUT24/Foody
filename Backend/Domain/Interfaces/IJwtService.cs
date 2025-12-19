using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user, IList<string> roles);
        Task RevokeAllAsync(User user, CancellationToken ct);
        Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshTokenValue, CancellationToken ct);
        Task<(string AccessToken, string RefreshToken)> ReissueTokensAsync(User user, CancellationToken ct);
        string Sha256(string input);
        string GenerateSecureToken();
    }
}
