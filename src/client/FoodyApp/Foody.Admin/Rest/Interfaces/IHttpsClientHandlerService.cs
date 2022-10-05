namespace Foody.Admin.Rest.Interfaces;

public interface IHttpsClientHandlerService
{
    HttpMessageHandler GetPlatformHandler();
}

