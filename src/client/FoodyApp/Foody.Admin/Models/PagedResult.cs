namespace Foody.Admin.Models;

public class PagedResult<T> : Result<IEnumerable<T>>
{
    public int Page { get; set; }

    public int ResultLimit { get; set; }

    public int ResultItems { get; set; }
}

