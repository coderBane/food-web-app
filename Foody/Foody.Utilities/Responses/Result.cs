using System;

namespace Foody.Utilities.Responses;

public class Result<T>
{
    public T? Content { get; set; }

    public Error? Error { get; set; }

    public bool Success => Error == null;

    public DateTime Date { get; set; } = DateTime.UtcNow;
}

