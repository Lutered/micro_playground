using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace UsersAPI.Extensions
{
    public static class IDistributedCacheExtension
    {
        private static readonly TimeSpan expirationTime = TimeSpan.FromMinutes(20);

        public static async Task<string> GetAsync(
            this IDistributedCache distributedCache, 
            string key)
        {
            var cacheData = await distributedCache.GetStringAsync(key);

            if(cacheData != null) await distributedCache.RefreshAsync(key);

            return cacheData;
        }

        public static async Task<T> GetObjectOrCreateAsync<T>(
            this IDistributedCache distributedCache, 
            string key, 
            Func<Task<T>> executeFn)
        {
            var cacheData = await distributedCache.GetStringAsync(key);

            if (cacheData != null)
            {
                await distributedCache.RefreshAsync(key);
                return JsonSerializer.Deserialize<T>(cacheData);
            }

            var data = await executeFn();

            cacheData = JsonSerializer.Serialize(data);

            await distributedCache.SetStringAsync(key, cacheData, new DistributedCacheEntryOptions
            {
                SlidingExpiration = expirationTime
            });

            return data;
        }

        public static async Task UpdateOrCreateAsync(this IDistributedCache distributedCache, string key, string value)
        {
            await distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                SlidingExpiration = expirationTime
            });
        }

        public static async Task<uint> GetVersionAsync(this IDistributedCache distributedCache, string key)
        {
            string fullKey = $"{key}:version";
            var versionStr = await distributedCache.GetStringAsync(fullKey);

            if(versionStr == null)
            {
                versionStr = "1";
                await distributedCache.SetStringAsync(fullKey, versionStr);
            }
            else await distributedCache.RefreshAsync(fullKey);

            return UInt32.Parse(versionStr);
        }

        public static async Task UpdateVersionAsync(this IDistributedCache distributedCache, string key)
        {
            string fullKey = $"{key}:version";
            var versionStr = await distributedCache.GetStringAsync(fullKey);

            if (versionStr == null) return;

            uint version = UInt32.Parse(versionStr) + 1;

            await distributedCache.SetStringAsync(fullKey, version.ToString());
        }
    }
}
