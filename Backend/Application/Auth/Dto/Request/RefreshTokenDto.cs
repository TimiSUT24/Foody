using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dto.Request
{
    public record RefreshTokenDto
    {
        public string RefreshToken { get; set; } = null!;
    }
}
