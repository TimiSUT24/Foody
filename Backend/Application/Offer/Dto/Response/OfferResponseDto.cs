using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offer.Dto.Response
{
    public record OfferResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal DiscountValue { get; init; }
        public DateTime StartsAtUtc  { get; init; }
        public DateTime EndsAtUtc { get; init; }
        public int ProductId { get; init; }
        public bool IsActive { get; init; }

    }
}
