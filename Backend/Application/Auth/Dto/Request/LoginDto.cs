using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dto.Request
{
    public record LoginDto(
        string Email,
        string Password
        );
}
