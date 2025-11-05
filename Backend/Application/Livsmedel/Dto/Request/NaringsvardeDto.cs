using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Livsmedel.Dto.Request
{
    public class NaringsvardeDto
    {
        [JsonPropertyName("namn")]
        public string Namn { get; set; } = string.Empty;

        [JsonPropertyName("euroFIRkod")]
        public string EuroFIRkod { get; set; } = string.Empty;
        public string Forkortning { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Enhet { get; set; } = string.Empty;
        public decimal ViktGram { get; set; }
        public string MatrisEnhet { get; set; } = string.Empty;
        public string MatrisEnhetKod { get; set; } = string.Empty;
        public string Berakning { get; set; } = string.Empty;
        public string VardeTyp { get; set; } = string.Empty;
        public string VardeTypKod { get; set; } = string.Empty;
        public string Ursprung { get; set; } = string.Empty;
        public string UrsprungKod { get; set; } = string.Empty;
        public string Publikation { get; set; } = string.Empty;
        public string MetodTyp { get; set; } = string.Empty;
        public string MetodTypKod { get; set; } = string.Empty;
        public string MetodIndikator { get; set; } = string.Empty;
        public string MetodIndikatorKod { get; set; } = string.Empty;
        public string ReferensTyp { get; set; } = string.Empty;
        public string ReferensTypKod { get; set; } = string.Empty;
        public string Kommentar { get; set; } = string.Empty;
    }
}
