using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Request
{
    public record ShippingInformation
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string Adress { get; init; }
        public required string City { get; init; }
        public required string State { get; init; }
        public required string PostalCode { get; init; }
        public required string PhoneNumber { get; init; }
    }
}
