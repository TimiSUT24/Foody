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
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _redisDb;

        public HybridCacheService(HybridCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redis = redis;
            _redisDb = redis.GetDatabase();
        }

        private async Task TrackKeyAsync(string prefix, string key)
        {
            await _redisDb.SetAddAsync($"cache-keys:{prefix}", key);
        }

        public async ValueTask<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan ttl)
        {
            await TrackKeyAsync("products:", key);
            await TrackKeyAsync("category:", key);
            return await _cache.GetOrCreateAsync(key, factory,
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
