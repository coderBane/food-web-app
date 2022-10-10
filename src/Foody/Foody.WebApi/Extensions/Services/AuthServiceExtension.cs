using System.Text;
using Foody.Auth.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Foody.WebApi.Extensions.Services;

public class AuthServiceExtension : IServiceExtension
{
    public void RenderService(IServiceCollection services, IConfiguration configuration)
    {
        // Update JWT class
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

        // Add TokenValidationParameters to DI
        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuer = false, // TODO Update to true
            ValidateAudience = false, // TODO Update to true
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"])),
        };

        services.AddSingleton(tokenValidationParams);

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

