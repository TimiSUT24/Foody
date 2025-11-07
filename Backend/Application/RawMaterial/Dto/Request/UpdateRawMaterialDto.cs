using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RawMaterial.Dto.Request
{
    public record UpdateRawMaterialDto(
        int Id,
        string Name,
        string? FoodEx2,
        string? Cooking,
        decimal? Portion,
        decimal? Factor,
        decimal? ConvertedToRow
        );
}
