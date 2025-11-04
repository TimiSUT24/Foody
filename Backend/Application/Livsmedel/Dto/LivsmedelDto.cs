using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto
{

    public class LivsmedelDto
    {
        [JsonPropertyName("livsmedelsTypId")]
        public int LivsmedelsTypId { get; set; }

        [JsonPropertyName("livsmedelsTyp")]
        public string LivsmedelsTyp { get; set; } = string.Empty;

        [JsonPropertyName("nummer")]
        public int Nummer { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;


        [JsonPropertyName("namn")]
        public string Namn { get; set; } = string.Empty;


        [JsonPropertyName("vetenskapligtNamn")]
        public string VetenskapligtNamn { get; set; } = string.Empty;


        [JsonPropertyName("projekt")]
        public string Projekt { get; set; } = string.Empty;

    }


}
