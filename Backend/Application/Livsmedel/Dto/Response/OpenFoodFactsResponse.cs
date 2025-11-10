using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto.Response
{
    public record OpenFoodFactsResponse
    {
        public List<OpenFoodFactsProduct> Products { get; set; } = new();
    }
}
