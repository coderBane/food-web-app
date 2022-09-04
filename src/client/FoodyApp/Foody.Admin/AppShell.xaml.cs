using Foody.Admin.Pages;

namespace Foody.Admin;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
	}
}

