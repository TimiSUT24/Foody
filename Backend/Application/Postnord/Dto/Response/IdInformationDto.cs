using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Postnord.Dto.Response
{
    public record IdInformationDto
    {
        public IdsDto[]? Ids { get; init; }
        public UrlsDto[]? Urls { get; init; }   
    }
}
