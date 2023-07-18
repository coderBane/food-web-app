using System.Text;
using Foody.Auth.Configuration;
using Foody.WebApi.Extensions.Services.Base;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Foody.Data.Data;

namespace Foody.WebApi.Extensions.Services;

public class AuthServiceExtension : IServiceExtension
{
    public void RenderService(IServiceCollection services, IConfiguration configuration)
    {
        // Update JWT class
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuer = false, // TODO Update to true
            ValidateAudience = false, // TODO Update to true
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"])),
            ClockSkew = TimeSpan.FromSeconds(5)
        };

        // Add TokenValidationParameters to DI
        services.AddSingleton(tokenValidationParams);

        // Add Identity DI
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodyDbContext>();

        // Add jwt authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwt =>
        {
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = tokenValidationParams;
        });
    }
}

