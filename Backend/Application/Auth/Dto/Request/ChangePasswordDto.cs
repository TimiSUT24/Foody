using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dto.Request
{
    public record ChangePasswordDto
    {
        public required string CurrentPassword { get; set;}
        public required string NewPassword { get; set; }
    }
}
