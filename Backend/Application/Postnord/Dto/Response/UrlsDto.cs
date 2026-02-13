using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Postnord.Dto.Response
{
    public record UrlsDto
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; init; } = string.Empty;
    }
}
