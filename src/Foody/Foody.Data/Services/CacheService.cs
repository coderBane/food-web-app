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
        var value = await _cachedB.StringGetAsync(key);
        return !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<T>(value!) : default;
    }

    public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expire = expirationTime.DateTime.Subtract(DateTime.Now);
        return await _cachedB.StringSetAsync(key, JsonSerializer.Serialize<T>(value), expire);
    }

    public async Task<bool> RemoveData(string key) =>
        await _cachedB.KeyExistsAsync(key) ? await _cachedB.KeyDeleteAsync(key) : false;
}

