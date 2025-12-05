using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klarna.Dto.Request
{
    public class KlarnaOrderLineDto
    {
        public string Type { get; set; } = "physical";
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Unit_price { get; set; }
        public int Total_amount { get; set; }
        public int Total_tax_amount { get; set; }
    }
}
