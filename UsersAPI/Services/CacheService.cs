using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UsersAPI.DTOs;
using UsersAPI.Helpers;
using UsersAPI.Interfaces;

namespace UsersAPI.Services
{
    public class CacheService(IDistributedCache _distributedCache) : ICacheService
    {
        private readonly TimeSpan absoluteTime = TimeSpan.FromMinutes(20);
        public async Task<string> GetCacheRawDataAsync(string key)
        {
            var cacheData = await _distributedCache.GetStringAsync(key);

            return cacheData;
        }

        public async Task<T> GetCacheObjectOrCreateAsync<T>(string key, Func<Task<T>> executeFn)
        {
            var cacheData = await _distributedCache.GetStringAsync(key);

            if (cacheData != null)
            {
                await _distributedCache.RefreshAsync(key);
                return JsonSerializer.Deserialize<T>(cacheData);
            }

            var data = await executeFn();

            cacheData = JsonSerializer.Serialize(data);

            await _distributedCache.SetStringAsync(key, cacheData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteTime
            });

            return data;
        }

        public async Task UpdateOrCreateRawDataAsync(string key, string value)
        {
            await _distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteTime
            }););
            await _distributedCache.RefreshAsync(key);

        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}
