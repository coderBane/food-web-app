using Foody.Admin.Authentication.Interfaces;


namespace Foody.Admin;

public partial class App : Application
{
	public static IAccountRepository Account { get; private set; }

	public App(IAccountRepository account)
	{
		InitializeComponent();

		Account = account;
		MainPage = new AppShell();
	}

    protected override async void OnStart()
    {
		if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
			await AppShell.Current.DisplayAlert("Network Access","Please check your internet connection","Ok");

        base.OnStart();
    }
}

