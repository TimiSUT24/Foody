using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StripeChargeShippingOptions.Dto
{
    public record StripeShippingDto2
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string PhoneNumber { get; init; } = null!;
        public string Adress { get; init; } = null!;
        public string City { get; init; } = null!;
        public string State { get; init; } = null!;
        public string PostalCode { get; init; } = null!;
        public string DeliveryOptionId { get; init; } = null!;
        public string ServiceCode { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}
