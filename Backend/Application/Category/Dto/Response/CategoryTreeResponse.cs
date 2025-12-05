using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Dto.Response
{
    public record CategoryTreeResponse
    {
        public int Id { get; init; }
        public string MainCategory { get; init; } = string.Empty;
        public List<SubCategoryTreeResponse>? SubCategories { get; init; }

    }
}
