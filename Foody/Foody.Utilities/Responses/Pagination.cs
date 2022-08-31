using System;

namespace Foody.Utilities.Responses;

public class Pagination<T> : Result<IEnumerable<T>>
{
    public Pagination(IEnumerable<T>? content)
    {
        Content = content is null ? Enumerable.Empty<T>() : content;
    }

    /// <summary>Current Page</summary>
    public int Page { get; set; }

    /// <summary>Items per page</summary>
    public int ResultLimit { get; set; }

    /// <summary>Total number of items in result list</summary>
    public int ResultItems => Content is not null ? Content.Count() : default;
}

