using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace UsersAPI.Extensions
{
    public static class IDistributedCacheExtension
    {
        private static readonly TimeSpan expirationTime = TimeSpan.FromMinutes(30);

        public static async Task<string> GetRawAsync(
            this IDistributedCache distributedCache, 
            string key)
        {
            return await distributedCache.GetStringAsync(key);
        }

        public static async Task<T> GetAsync<T>(
            this IDistributedCache distributedCache,
            string key)
        {
            var cacheData = await distributedCache.GetStringAsync(key);

            if (cacheData is null) return default(T);

            return JsonSerializer.Deserialize<T>(cacheData);
        }

        public static async Task<bool> CreateRawAsync(
            this IDistributedCache distributedCache,
            string key,
            string value
            )
        {
            try
            {
                await distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = expirationTime
                });

                return true;
            }
            catch { return false; }
        }

        public static async Task<bool> CreateAsync<T>(
            this IDistributedCache distributedCache,
            string key,
            T value)
        {
            var cacheData = JsonSerializer.Serialize(value);

            return await distributedCache.CreateRawAsync(key, cacheData);
        }

        public static async Task<uint> GetVersionAsync(
            this IDistributedCache distributedCache, 
            string key
        )
        {
            string fullKey = $"{key}:version";
            var versionStr = await distributedCache.GetStringAsync(fullKey);

            if(versionStr == null)
            {
                versionStr = "1";
                await distributedCache.SetStringAsync(fullKey, versionStr,
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = expirationTime.Add(expirationTime)
                    });
            }

            return UInt32.Parse(versionStr);
        }

        public static async Task UpdateVersionAsync(
            this IDistributedCache distributedCache, 
            string key)
        {
            string fullKey = $"{key}:version";
            var versionStr = await distributedCache.GetStringAsync(fullKey);

            if (versionStr == null) return;

            uint version = UInt32.Parse(versionStr) + 1;

            await distributedCache.SetStringAsync(fullKey, version.ToString());
        }
    }
}
