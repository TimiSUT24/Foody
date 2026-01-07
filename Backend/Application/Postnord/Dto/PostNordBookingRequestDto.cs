using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Postnord.Dto
{
    public record PostNordBookingRequestDto
    {
        [JsonPropertyName("shipping")]
        public ShippingDto Shipping { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public decimal TotalWeight { get; set; }
    }
}
