using Foody.Admin.Pages;
using Foody.Admin.ViewModels;

namespace Foody.Admin;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		BindingContext = this;

		//Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));

		// Category
		Routing.RegisterRoute("categorydeet", typeof(Pages.Category.DetailsPage));
		Routing.RegisterRoute("categorymod", typeof(Pages.Category.ModifyPage));
	}

    [ICommand]
	async void Logout() => await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
}

