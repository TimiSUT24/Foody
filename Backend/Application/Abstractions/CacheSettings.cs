using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public record CacheSettings
    {
        public int LongLivedMinutes { get; set; }
        public int ShortLivedMinutes { get; set; }
    }
}
