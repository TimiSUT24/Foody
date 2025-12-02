using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klarna.Dto.Request
{
    public class KlarnaMerchantUrlsDto
    {
        public string Checkout { get; set; } = string.Empty;
        public string Confirmation { get; set; } = string.Empty;
        public string Push { get; set; } = string.Empty;
    }
}
