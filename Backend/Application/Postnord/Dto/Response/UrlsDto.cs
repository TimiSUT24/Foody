using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Postnord.Dto.Response
{
    public record UrlsDto
    {
        public string Type { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
    }
}
