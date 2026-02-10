using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Request
{
    public class RetrievePaymentIntentRequest
    {
        public string ClientSecret { get; set; } = string.Empty;

    }
}