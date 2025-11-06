using Application.Auth.Dto.Request;
using Application.Auth.Dto.Response;
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
    }
}
