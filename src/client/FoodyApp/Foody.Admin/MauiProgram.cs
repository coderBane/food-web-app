using Foody.Admin.ViewModels.Category;
using Foody.Admin.Authentication.Services;
using Foody.Admin.Authentication.Interfaces;
using Foody.Admin.Rest.Services;
using Foody.Admin.ViewModels.Contact;

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
		builder.Services.AddScoped<IDataManager, DataManager>();
        builder.Services.AddSingleton<IAccountRepository, AccountService>();
        builder.Services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();

		//ViewModels
		builder.Services.AddSingleton<CategoryVM>();
		builder.Services.AddTransient<CategoryDetailsVM>();
		builder.Services.AddTransient<CatogoryModifyVM>();
		builder.Services.AddSingleton<ContactVM>();

        #region Pages
        #region Category
        builder.Services.AddSingleton<Pages.Category.CategoryPage>();
		builder.Services.AddTransient<Pages.Category.DetailsPage>();
		builder.Services.AddTransient<Pages.Category.ModifyPage>();
		#endregion
		#region Contact
		builder.Services.AddSingleton<Pages.Contact.ContactPage>();
        #endregion
        #endregion


        return builder.Build();
	}
}

