namespace Foody.Admin.Authentication.Models;

public class AccountResponse
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public bool Success { get; set; }

    public IEnumerable<string> Errors { get; set; }
}


