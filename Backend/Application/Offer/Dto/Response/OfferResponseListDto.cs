using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offer.Dto.Response
{
    public record OfferResponseListDto
    {
        public List<OfferResponseDto>? Offer {  get; init; }
    }
}
