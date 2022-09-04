using Foody.Admin.Pages;
using Foody.Admin.Authentication.Services;
using Foody.Admin.Authentication.Interfaces;


namespace Foody.Admin;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		//builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

		// REST
		builder.Services.AddSingleton<IAccountRepository, AccountService>();
		builder.Services.AddScoped<IDataManager, DataManager>();

		//ViewModels
		builder.Services.AddSingleton<CategoryVM>();

		//Pages
		builder.Services.AddSingleton<CategoryPage>();

		return builder.Build();
	}
}

