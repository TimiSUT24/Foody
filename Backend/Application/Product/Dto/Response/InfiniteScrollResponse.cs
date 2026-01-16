using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Response
{
    public record InfiniteScrollResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public bool HasMore { get; set; }
    }
}
