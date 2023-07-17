﻿namespace Foody.WebApi.Extensions.Services.Base;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        typeof(Program).Assembly.ExportedTypes
            .Where(ex => typeof(IServiceExtension).IsAssignableFrom(ex) && !ex.IsInterface && !ex.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IServiceExtension>()
            .ToList().ForEach(service => service.RenderService(services, configuration));
    }
}

