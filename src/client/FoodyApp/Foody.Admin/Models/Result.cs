namespace Foody.Admin.Models;

public class Result<T>
{
    public T Content { get; set; }

    public Error Error { get; set; }

    public bool Success { get; set; }

    public DateTime Date { get; set; } 
}

