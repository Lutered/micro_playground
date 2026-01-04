using Microsoft.Extensions.Caching.Distributed;
using Shared.Configurations;
using UsersAPI.Data.Entities;

namespace UsersAPI.Helpers
{
    public class UserCacheHelper(IDistributedCache _cache)
    {
        public async Task ClearUserCache(User user)
        {
            await _cache.RemoveAsync($"{CacheKeys.User.UserId}:{user.Id}");
            await _cache.RemoveAsync($"{CacheKeys.User.UserName}:{user.Username}");
        }
    }
}
