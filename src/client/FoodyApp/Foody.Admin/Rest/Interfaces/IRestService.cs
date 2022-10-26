namespace Foody.Admin.Rest.Interfaces;

public interface IRestService<T> where T : class, IEntity
{
    Task<PagedResult<T>> AllAsync();

    Task<Result<T>> GetAsync(int id);

    Task<Result<dynamic>> SaveDataAsync(T entity, bool isNew);

    Task<Result<dynamic>> DeleteAsync(int id);
}

