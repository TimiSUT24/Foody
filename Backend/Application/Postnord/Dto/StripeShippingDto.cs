using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Postnord.Dto
{
    public record StripeShippingDto
    {
        public AddressDto Address { get; set; } = null!;
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = null!;
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }

}
