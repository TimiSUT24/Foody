using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dto.Request
{
    public record UpdateProfileDto
    {
        [DefaultValue("")]
        public string? FirstName { get; set; }
        [DefaultValue("")]
        public string? LastName { get; set; }
        [DefaultValue("")]
        public string? Email { get; set; }
        [DefaultValue("")]
        public string? PhoneNumber { get; set; } 
    }
}
