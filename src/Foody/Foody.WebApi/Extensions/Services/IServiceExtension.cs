namespace Foody.WebApi.Extensions.Services;

public interface IServiceExtension
{
    void RenderService(IServiceCollection services, IConfiguration configuration);
}

