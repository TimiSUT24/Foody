using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classification.Dto.Request
{
    public record UpdateClassificationDto(
        int Id,
        string? Type,
        string? Facet,
        string? FacetCode,
        string? Code,
        string? Name,
        string? LangualId
        );
}
