using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klarna.Dto.Request
{
    public class KlarnaCreateOrderRequestDto
    {
        public string Purchase_country { get; set; } = string.Empty;
        public string Purchase_currency { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;

        public int Order_amount { get; set; }
        public int Order_tax_amount { get; set; }

        public List<KlarnaOrderLineDto> Order_lines { get; set; } = new();

        public KlarnaMerchantUrlsDto Merchant_urls { get; set; } = new();
    }
}
