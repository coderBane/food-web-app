namespace Foody.Admin.Rest.Services;

public class CategoryService : RestService<Category>
{
    public CategoryService(IHttpsClientHandlerService httpsClientHandler) : base(httpsClientHandler)
    {
        url = Address.Category.BaseAddress;
    }

    public override async Task<Result<dynamic>> SaveDataAsync(Category entity, bool isNew)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(entity.Name), nameof(entity.Name) },
                { new StringContent($"{entity.IsActive}"), nameof(entity.IsActive) }
            };

            if (entity.ImageUpload is not null)
                content.Add(entity.ImageUpload, "file", nameof(entity.ImageUpload));

            var response = isNew ? await _client.PostAsync(url, content) :
                await _client.PutAsync($"{url}/{entity.Id}", content);

            single = response.StatusCode is System.Net.HttpStatusCode.NoContent ? null :
                await response.Content.ReadFromJsonAsync<Result<dynamic>>();

            if (single is not null && !single.Success)
                Debug.WriteLine($"{single.Error.Title} : {single.Error.Message}");
        }
        catch (Exception)
        {
            throw;
        }

        return single;
    }
}

