using Foody.Data.Data;
using Foody.Data.Services;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Foody.WebApi.Extensions.Services;

public class DataServiceExtension : IServiceExtension
{
    public void RenderService(IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext DI
        services.AddDbContext<FoodyDbContext>(options =>
            options.UseNpgsql(configuration.GetValue<string>("Postgres"))
               .UseSnakeCaseNamingConvention());

        // Add Identity DI
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodyDbContext>();

        // Add UnitofWork DI
        services.AddScoped<IUnitofWork, UnitofWork>();

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
            .Connect(configuration.GetValue<string>("Redis")));

        // Add Caching DI
        services.AddSingleton<ICacheService, CacheService>();
    }
}

