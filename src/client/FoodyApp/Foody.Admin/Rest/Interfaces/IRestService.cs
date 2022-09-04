namespace Foody.Admin.Rest.Interfaces;

public interface IRestService<T> where T : class
{
    Task<PagedResult<T>> AllAsync(string search);

    Task<T> GetAsync(int id);
}

