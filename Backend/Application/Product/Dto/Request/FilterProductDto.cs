using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Request
{
    public record FilterProductDto
    {
        public string? Brand { get; init; }
        public int? CategoryId { get; init; }
        public int? SubCategoryId { get; init; }
        public int? SubSubCategoryId { get; init; }  
        public decimal? Price { get; init; }

    }
}
