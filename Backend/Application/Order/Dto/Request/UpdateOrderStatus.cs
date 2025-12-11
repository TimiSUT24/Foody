using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Request
{
    public record UpdateOrderStatus
    {
        public Guid Id { get; set; }
        public string? OrderStatus { get; set; } = string.Empty;
        public string? PaymentStatus { get; set; } = string.Empty;
    }
}
