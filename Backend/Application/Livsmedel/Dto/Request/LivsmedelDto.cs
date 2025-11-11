using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto.Request
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
        [JsonPropertyName("tillagningsmetod")]
        public string Tillagningsmetod { get; set; } = string.Empty;
        [JsonPropertyName("analys")]
        public string Analys { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }


    }


}
