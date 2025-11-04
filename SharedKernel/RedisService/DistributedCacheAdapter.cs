using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisService
{
    public class DistributedCacheAdapter : ICacheAdapter
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheAdapter(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T Get<T>(string key)
         {
            var data = _cache.GetString(key);
            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data);
        }

        public void Set<T>(string key, T value)
        {
            var data = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(30) };
            _cache.SetString(key, data, options);
        }
    }
}
