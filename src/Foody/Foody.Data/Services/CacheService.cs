using System.Text.Json;
using StackExchange.Redis;

namespace Foody.Data.Services
{
    public sealed class CacheService : ICacheService
    {
        IDatabase _cachedB;

        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            _cachedB = redis.GetDatabase();
        }

        public T? GetData<T>(string key)
        {
            var value = _cachedB.StringGet(key);
            return !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<T>(value!) : default;
        }

        public bool RemoveData(string key) => _cachedB.KeyExists(key) ? _cachedB.KeyDelete(key) : false;

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expire = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cachedB.StringSet(key, JsonSerializer.Serialize<T>(value), expire);
        }
    }
}

