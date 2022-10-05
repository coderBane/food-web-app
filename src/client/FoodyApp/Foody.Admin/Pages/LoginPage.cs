using Foody.Admin.Authentication.Models;
using Image = Microsoft.Maui.Controls.Image;

namespace Foody.Admin.Pages;

public class LoginPage : ContentPage
{
    Entry uEntry, pEntry;

	public LoginPage()
	{
        uEntry = new Entry() { Placeholder = "email or username" };
        pEntry = new Entry() { Placeholder = "password", IsPassword = true };

        Content = new VerticalStackLayout
        {
            Spacing  = 25,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,

            Children =
            {
                new VerticalStackLayout
                {
                    Spacing = 5,

                    Children =
                    {
                        new Frame
                        {
                            CornerRadius = 28,
                            HeightRequest = 100,
                            WidthRequest = 100,

                            Content = new Image
                            {
                                Source = "admin_settings_panel.png",
                                HeightRequest = 60,
                                WidthRequest = 60,
                            }
                        },
                        new Label
                        {
                            Text = "Foody Administrator",
                            FontSize = 30,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = TextAlignment.Center
                        },
                        new Label
                        {
                            Text = "Login to access Dashboard",
                            FontSize = 18,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                },
                new VerticalStackLayout
                {
                    Spacing = 5,

                    Children =
                    {
                        new Label
                        {
                            Text = "Username",
                            FontSize = 15
                        },
                        uEntry
                    }
                },
                new VerticalStackLayout
                {
                    Spacing = 5,

                    Children =
                    {
                        new Label
                        {
                            Text = "Password",
                            FontSize = 15
                        },
                        pEntry
                    }
                },
                new Button
                {
                    Text = "LOG IN",
                    Command = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}"))
                }
            }
        };

        //Shell.SetNavBarIsVisible(Content, false);
    }

    async void Submit()
    {
        var response = await App.Account.Authenticate(new Login
        {
            Username = uEntry.Text,
            Password = pEntry.Text
        });

        if (!response.Item1)
            if (response.Item2 is not null)
                await DisplayAlert("Authentication Failed", $"{response.Item2.Errors.ToString()}", "Try Again");
            else
                await DisplayAlert("Authentication Failed", $"{response.Item3}","Try Again");
        else
            await DisplayAlert("Success", "", "Ok");
    }
}

