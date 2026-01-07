using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Postnord.Dto
{
    public record AddressDto
    {
        public string Line1 { get; set; } = null!;
        public string Postal_Code { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
