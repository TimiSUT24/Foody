using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Postnord.Dto.Response
{
    public record IdInformationDto
    {
        [JsonPropertyName("ids")]
        public IdsDto[]? Ids { get; init; }
        [JsonPropertyName("urls")]
        public UrlsDto[]? Urls { get; init; }
        [JsonPropertyName("status")]
        public string? Status { get; init; }
    }
}
