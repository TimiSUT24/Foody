using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dto.Request
{
    public record RegisterDto(
        string UserName,
        string? FirstName,
        string? LastName,
        string Email,
        string? PhoneNumber,
        string Password
        );

}
