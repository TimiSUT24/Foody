using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Postnord.Dto
{
    public record ValidationPostalCode
    {
        public string ValidationResult { get; set; } = string.Empty;
    }
}
