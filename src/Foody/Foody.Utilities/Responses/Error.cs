using System;

namespace Foody.Utilities.Responses;

public sealed class Error
{
    /// <summary>Status Code</summary>
    public int Code { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}

