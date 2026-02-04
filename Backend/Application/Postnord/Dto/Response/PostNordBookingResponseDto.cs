using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Postnord.Dto.Response
{
    public record PostNordBookingResponseDto
    {
        public IdInformationDto? IdInformation { get; init; }
        public string BookingId { get; init; } = string.Empty;
    }
}
