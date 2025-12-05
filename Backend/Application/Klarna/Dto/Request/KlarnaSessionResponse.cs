using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Klarna.Dto.Request
{
    public record KlarnaSessionResponse
    {
        public string ClientToken { get; init; } = null!;
        public string SessionId { get; init; } = null!;
        public JsonElement PaymentMethodCategories { get; init; }
        public object Payload { get; init; } = null!;

    }
        
}
