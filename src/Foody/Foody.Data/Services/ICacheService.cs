namespace Foody.Data.Services;

public interface ICacheService
{
    Task<T?> GetData<T>(string key);

    Task<bool> SetData<T>(string key, T Value, DateTimeOffset expirationTime);

    Task<bool> RemoveData(string key);
}

