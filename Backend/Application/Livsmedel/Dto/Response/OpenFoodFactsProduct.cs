using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto.Response
{
    public record OpenFoodFactsProduct
    {
        public string? ProductName { get; init; }
        public string? ImageUrl { get; init; }
    }
}
