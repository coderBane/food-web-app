namespace Foody.Admin.Rest.Interfaces;

public interface IRestService<T> where T : class
{
    Task<PagedResult<T>> AllAsync(string search);

    Task<Result<dynamic>> GetAsync(int id);

    Task<Result<dynamic>> SaveDataAsync(T entity, bool isNew = false);

    Task<Result<T>> DeleteAsync(int id);
}

