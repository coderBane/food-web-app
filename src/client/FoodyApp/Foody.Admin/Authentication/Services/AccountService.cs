using Foody.Admin.Authentication.Models;
using Foody.Admin.Authentication.Interfaces;
using System.Net.Http.Headers;

namespace Foody.Admin.Authentication.Services;

public class AccountService : IAccountRepository
{
    AccountResponse response;
    readonly HttpClient _client = new();
    readonly JsonSerializerOptions _serializerOptions;

    public AccountService()
    {
        _client.BaseAddress = new Uri(Address.Base.BaseAddress);

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<(bool, AccountResponse, string)> Authenticate(Login login)
    {
        //var data = JsonSerializer.Serialize(login, _serializerOptions);
        var json = JsonContent.Create(login, mediaType: MediaTypeHeaderValue.Parse("application/json"), options: _serializerOptions);
        //var content = new StringContent(data, Encoding.UTF8, "application/json");

        string message;

        try
        {
            var result = await _client.PostAsync(Address.Account.LoginAddress, json);
            result.EnsureSuccessStatusCode();
            response = await result.Content.ReadFromJsonAsync<AccountResponse>();

            if (!response.Success)
                Debug.WriteLine(response.Errors.ToString());

            return (response.Success, response, null);
        }
        catch (Exception ex)
        {
            message = ex.Message;
            Debug.WriteLine(ex.Message);
        }

        return (false, null, message);
    }
}

