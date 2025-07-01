namespace UsersAPI.Interfaces
{
    public interface ICacheService
    {
        public Task<string> GetCacheRawDataAsync(string key);
        public Task<T> GetCacheObjectOrCreateAsync<T>(string key, Func<Task<T>> executeFn);
        public Task UpdateOrCreateRawDataAsync(string key, string value);
        public Task RemoveAsync(string key);
    }
}
