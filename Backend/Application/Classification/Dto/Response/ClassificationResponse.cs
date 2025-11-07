using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classification.Dto.Response
{
    public record ClassificationResponse
    {
        public int Id { get; init; }
        public string? Type { get; init; }
        public string? Facet { get; init; }
        public string? FacetCode { get; init; }
        public string? Code { get; init; }
        public string? Name { get; init; }
        public string? LangualId { get; init; }
    }
}
