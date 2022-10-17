using Foody.Data.Data;
using Foody.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

/***Add services to the container.***/

builder.Services.ConfigureServices(builder.Configuration);

// AutoMapper DI
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

/*** Initialize Database ***/
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var password = builder.Configuration.GetValue<string>("UserPW");
    await DbInitialize.InitializeAsync(services, password);
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

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

