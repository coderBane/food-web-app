using System.Text;

using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Auth.Configuration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.

//Update JWT class
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

//Add DbContext DI
builder.Services.AddDbContext<FoodyDbContext>(option =>
    option.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodyDbContext>();

//Add UnitofWork DI
builder.Services.AddScoped<IUnitofWork, UnitofWork>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default;
});

// Add TokenValidationParameters to DI
var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuer = false, // TODO Update to true
    ValidateAudience = false, // TODO Update to true
    ValidateLifetime = true,
    RequireExpirationTime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"])),
};

builder.Services.AddSingleton(tokenValidationParams);

//  Add jwt authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParams;
});

// AutoMapper DI
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

/*** Initialize Database ***/
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    using var content = services.GetRequiredService<FoodyDbContext>();
    DbInitialize.Initialize(content);
}
/****************************/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
