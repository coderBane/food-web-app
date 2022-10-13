using Foody.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Foody.WebApi.Extensions.Services;

public class ApiServiceExtension : IServiceExtension
{
    public void RenderService(IServiceCollection services, IConfiguration configuration)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = ApiVersion.Default;
        });

        services.AddControllers(options =>
        {
            //options.Filters.Add(new NullableAttribute());
            options.Filters.Add<ValidateModelAttribute>();
        }).AddJsonOptions(option =>
        {
            //option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });
    }
}

