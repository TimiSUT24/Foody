using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Postnord.Dto
{
    public record ShippingDto
    {
        public string ServiceCode { get; set; } = null!;
        [JsonPropertyName("email")]
        public string Email { get; init; } = null!;
        [JsonPropertyName("lastname")]
        public string Lastname { get; init; } = null!;
        [JsonPropertyName("shipping")]
        public StripeShippingDto Shipping { get; set; } = null!;
    }
}
