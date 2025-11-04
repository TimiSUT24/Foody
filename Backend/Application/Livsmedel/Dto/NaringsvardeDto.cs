using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto
{
    public class NaringsvardeDto
    {
        [JsonPropertyName("namn")]
        public string Namn { get; set; } = string.Empty;

        [JsonPropertyName("euroFIRkod")]
        public string EuroFIRkod { get; set; } = string.Empty;
    }
}
