using Foody.Admin.Authentication.Models;

namespace Foody.Admin.Authentication.Interfaces;

public interface IAccountRepository
{
    Task<(bool, AccountResponse, string)> Authenticate(Login login);
}

