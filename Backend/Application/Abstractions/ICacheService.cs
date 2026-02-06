using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface ICacheService
    {
        ValueTask<T> GetOrCreateAsync<T>(string prefix, string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan ttl);
        ValueTask RemoveAsync(string key);
        ValueTask RemoveByPrefixAsync(string prefix);
    }
}
