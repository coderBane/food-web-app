namespace Foody.Admin.Rest.Services;

public class RestService<T> : IRestService<T> where T : class, IEntity
{
    protected string url;
    protected readonly HttpClient _client;
    readonly IHttpsClientHandlerService _httpsClientHandler;

    PagedResult<T> pagedResult;
    Result<T> result;
    internal Result<dynamic> single;

    public RestService(IHttpsClientHandlerService httpsClientHandler)
    {
#if DEBUG
        _httpsClientHandler = httpsClientHandler;
        _httpsClientHandler.Initialize(out _client);
        if (_client is null)
            _client = new();
#else
        _client = new();
#endif
        _client.BaseAddress = new Uri(Address.Base.BaseAddress); 
    }

    public async Task<PagedResult<T>> AllAsync()
    {
        try
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<T>>();

            if (!pagedResult.Success)
                Debug.WriteLine($"{pagedResult.Error.Title} : {pagedResult.Error.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return pagedResult;
    }

    public async Task<Result<T>> GetAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync($"{url}/{id}");
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadFromJsonAsync<Result<T>>();

            if (!result.Success)
                Debug.WriteLine($"{result.Error.Title} : {result.Error.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return result;
    }

    public virtual async Task<Result<dynamic>> SaveDataAsync(T entity, bool isNew)
    {
        try
        {
            string json = entity.ToString();

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = isNew ? await _client.PostAsync(url, content) :
                               await _client.PutAsync($"{url}/{entity.Id}", content);

            response.EnsureSuccessStatusCode();
            single = await response.Content.ReadFromJsonAsync<Result<dynamic>>();

            if (!single.Success)
                Debug.WriteLine($"{result.Error.Title} : {result.Error.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return single;
    }

    public async Task<Result<dynamic>> DeleteAsync(int id)
    {
        var response = await _client.DeleteAsync($"{url}/{id}");
        response.EnsureSuccessStatusCode();
        single = response.StatusCode is System.Net.HttpStatusCode.NoContent ? null :
            await response.Content.ReadFromJsonAsync<Result<dynamic>>();

        if (single is not null)
            if (!single.Success)
                Debug.WriteLine($"{result.Error.Title} : {result.Error.Message}");
        
        return single;
    }
}

