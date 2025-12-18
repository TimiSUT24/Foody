using Application.Auth.Dto.Request;
using Application.Auth.Dto.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto request);
        Task<LoginDtoResponse> LoginAsync(LoginDto request, CancellationToken ct);
        Task<UpdateProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileDto request, CancellationToken ct);
        Task<UpdateProfileResponse> ChangePassword(Guid userId, ChangePasswordDto request,CancellationToken ct);
        Task Logout(Guid userId, CancellationToken ct);
        Task<(string AccessToken, string RefreshToken)> ReissueTokensAsync(User user, CancellationToken ct);
    }
}
