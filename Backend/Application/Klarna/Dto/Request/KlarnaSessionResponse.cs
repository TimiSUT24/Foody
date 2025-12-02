using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Klarna.Dto.Request
{
    public class KlarnaSessionResponse
    {
        public string Client_token { get; set; } = string.Empty;
        public string Html_snippet { get; set; } = string.Empty;
    }
}
