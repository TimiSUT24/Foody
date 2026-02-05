using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Postnord.Dto.Response
{
    public record PostNordBookingResponseDto
    {
        [JsonPropertyName("idInformation")]
        public IdInformationDto[]? IdInformation { get; init; }
        [JsonPropertyName("bookingId")]
        public string BookingId { get; init; } = string.Empty;
    }
}
