using Foody.Data.Data;
using Foody.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.

var Configurations = builder.Configuration;

//Add DbContext DI
builder.Services.AddDbContext<FoodyDbContext>(option =>
    option.UseSqlite(Configurations.GetConnectionString("Default")));

//Add UnitofWork DI
builder.Services.AddScoped<IUnitofWork, UnitofWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

/*** Initialize Database ***/
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    using var content = services.GetRequiredService<FoodyDbContext>();
    DbInitialize.Initialize(content);
}
 /*****/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
