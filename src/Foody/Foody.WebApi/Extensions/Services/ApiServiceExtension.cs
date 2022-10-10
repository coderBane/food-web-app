using Microsoft.AspNetCore.Mvc;

namespace Foody.WebApi.Extensions.Services;

public class ApiServiceExtension : IServiceExtension
{
    public void RenderService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = ApiVersion.Default;
        });
    }
}

