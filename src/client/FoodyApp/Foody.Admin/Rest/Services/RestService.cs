namespace Foody.Admin.Rest.Services;

public class RestService<T> : IRestService<T> where T : class
{
    readonly HttpClient _client = new();
    protected string url;

    PagedResult<T> result;

    public RestService()
    {
        _client.BaseAddress = new Uri(Address.Base.BaseAddress);
    }

    public async Task<PagedResult<T>> AllAsync(string search)
    {
        if (result is not null) return result;

        this.url += !string.IsNullOrEmpty(search) ? $"?search={search}" : default;

        try
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadFromJsonAsync<PagedResult<T>>();

            if (!result.Success)
                Debug.WriteLine($"{result.Error.Title} : {result.Error.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return result;
    }

    public Task<T> GetAsync(int id)
    {
        throw new NotImplementedException();
    }
}

