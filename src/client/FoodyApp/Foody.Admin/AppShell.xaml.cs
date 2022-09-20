using Foody.Admin.Pages;

namespace Foody.Admin;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));

		// Category
		Routing.RegisterRoute("categorydeet", typeof(Pages.Category.DetailsPage));
		Routing.RegisterRoute("categorymod", typeof(Pages.Category.ModifyPage));
	}

	async void logout_Clicked(Object sender, EventArgs e) =>
		await Shell.Current.GoToAsync("LoginPage", true);
}

