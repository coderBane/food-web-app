namespace Foody.Auth.Models;

public class AuthResult
{
    public string? Token { get; set; }

    public bool Success { get; set; }
    
    public IEnumerable<string> Errors { get; set; }

    public AuthResult() => Errors = Enumerable.Empty<string>();
}
