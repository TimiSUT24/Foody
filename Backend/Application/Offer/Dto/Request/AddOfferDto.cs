using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offer.Dto.Request
{
    public record AddOfferDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public DiscountType DiscountType { get; set; }

        public decimal DiscountValue { get; set; }
        public DateTime StartsAtUtc { get; set; }
        public DateTime EndsAtUtc { get; set; }

    }
}
