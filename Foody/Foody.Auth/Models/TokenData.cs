namespace Foody.Auth.Models;

public class TokenData
{
    public string JwtToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}

