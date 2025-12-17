using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Request
{
    public record ShippingPatchDto
    {
        public string? ShipmentId { get; set; }
        public string? TrackingId { get; set; }
        public string? TrackingUrl { get; set; }
        public string? Carrier { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Adress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
    }
}
