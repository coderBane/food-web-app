using Foody.Admin.Pages;
using Foody.Admin.ViewModels.Category;
using Foody.Admin.Authentication.Services;
using Foody.Admin.Authentication.Interfaces;

using System.Net.Http;

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
		builder.Services.AddTransient<CategoryDetailsVM>();
		builder.Services.AddTransient<CatogoryModifyVM>();

		//Pages
		builder.Services.AddSingleton<Pages.Category.CategoryPage>();
		builder.Services.AddTransient<Pages.Category.DetailsPage>();
		builder.Services.AddTransient<Pages.Category.ModifyPage>();

		return builder.Build();
	}
}

