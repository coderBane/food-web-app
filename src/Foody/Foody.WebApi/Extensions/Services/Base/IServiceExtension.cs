namespace Foody.WebApi.Extensions.Services.Base;

public interface IServiceExtension
{
    void RenderService(IServiceCollection services, IConfiguration configuration);
}

