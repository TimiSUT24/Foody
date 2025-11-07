using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RawMaterial.Dto.Response
{
    public class RawMaterialResponse
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? FoodEx2 { get; set; }
        public string? Cooking { get; set; }
        public decimal? Portion { get; set; }
        public decimal? Factor { get; set; }
        public decimal? ConvertedToRaw { get; set; }
    }
}
