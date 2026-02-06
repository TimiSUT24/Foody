using Application.Abstractions;
using Microsoft.Extensions.Caching.Hybrid;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public class HybridCacheService : ICacheService
    {
        private readonly HybridCache _cache;
        private readonly IDatabase _redisDb;

        public HybridCacheService(HybridCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redisDb = redis.GetDatabase();
        }

        private async Task TrackKeyAsync(string prefix, string fullKey)
        {
            await _redisDb.SetAddAsync($"cache-keys:{prefix}", fullKey);
        }

        public async ValueTask<T> GetOrCreateAsync<T>(string prefix,string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan ttl)
        {
            var fullKey = $"{prefix}{key}";
            await TrackKeyAsync(prefix, fullKey);

            return await _cache.GetOrCreateAsync(fullKey, factory,
                new HybridCacheEntryOptions
                {
                    Expiration = ttl,
                });
        }  

        public async ValueTask RemoveByPrefixAsync(string prefix)
        {
            var indexKey = $"cache-keys:{prefix}";
            var keys = await _redisDb.SetMembersAsync(indexKey);

            foreach(var key in keys)
            {
                await _cache.RemoveAsync(key!);
            }
            await _redisDb.KeyDeleteAsync(indexKey);

        }
        
        public ValueTask RemoveAsync(string key)
         => _cache.RemoveAsync(key);


    }
}
