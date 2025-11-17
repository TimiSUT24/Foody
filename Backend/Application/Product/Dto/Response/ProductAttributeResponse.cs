using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Response
{
    public record ProductAttributeResponse
    {
        public int Id { get; init; }
        public string? Value { get; init; } = string.Empty;
    }
}
