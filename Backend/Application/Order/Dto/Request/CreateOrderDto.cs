using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Request
{
    public record CreateOrderDto
    {
        public required List<CreateOrderItemDto> Items { get; init; }
        public required ShippingInformation ShippingInformation { get; init; }
        public string? ServiceCode { get; set; } = string.Empty;
        public required string PaymentIntentId { get; set; } = string.Empty;
    }
}
