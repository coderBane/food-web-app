using System.Text.Json;
using StackExchange.Redis;

namespace Foody.Data.Services;

public sealed class CacheService : ICacheService
{
    private readonly IDatabase _cachedB;

    private readonly IConnectionMultiplexer _connection;

    public CacheService(IConnectionMultiplexer connection)
    {
        _connection = connection;
        _cachedB = _connection.GetDatabase();
    }

    public async Task<T?> GetData<T>(string key)
    {
        if (!_cachedB.IsConnected(key)) return default;
        var value = await _cachedB.StringGetAsync(key);
        return !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<T>(value!) : default;
    }

    public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        if (!_cachedB.IsConnected(key)) return false;
        var expire = expirationTime.DateTime.Subtract(DateTime.Now);
        return await _cachedB.StringSetAsync(key, JsonSerializer.Serialize(value), expire);
    }

    public async Task<bool> RemoveData(string key) => _cachedB.IsConnected(key) && await _cachedB.KeyDeleteAsync(key);
}

