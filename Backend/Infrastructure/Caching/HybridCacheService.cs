using Application.Abstractions;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public class HybridCacheService : ICacheService
    {
        private readonly HybridCache _cache;

        public HybridCacheService(HybridCache cache)
        {
            _cache = cache;
        }

        public ValueTask<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan ttl)
        {
            return _cache.GetOrCreateAsync(key, factory,
                new HybridCacheEntryOptions
                {
                    Expiration = ttl,
                });
        }   
        
        public ValueTask RemoveAsync(string key)
         => _cache.RemoveAsync(key);


    }
}
