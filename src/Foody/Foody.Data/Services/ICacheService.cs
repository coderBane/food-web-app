namespace Foody.Data.Services;

public interface ICacheService
{
    T? GetData<T>(string key);

    bool SetData<T>(string key, T Value, DateTimeOffset expirationTime);

    bool RemoveData(string key);
}

