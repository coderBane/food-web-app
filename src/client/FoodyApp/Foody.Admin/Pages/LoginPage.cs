using Foody.Admin.Authentication.Models;
using Image = Microsoft.Maui.Controls.Image;


namespace Foody.Admin.Pages;

public class LoginPage : ContentPage
{
    private readonly Entry username;
    private readonly Entry password;

    public LoginPage()
	{
        username = new Entry() { Placeholder = "email or username" };
        password = new Entry() { Placeholder = "password", IsPassword = true };

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
                        username
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
                        password
                    }
                },
                new Button
                {
                    Text = "LOG IN",
                    Command = new Command(async () => await Submit())
                }
            }
        };
    }

    async Task Submit()
    {
        if (!string.IsNullOrWhiteSpace(username.Text) && !string.IsNullOrWhiteSpace(password.Text))
        {
            var (success, response, message) = await App.Account.Authenticate(new Login
            {
                Email = username.Text,
                Password = password.Text
            });

            if (!success)
            {
                if (response is not null)
                    await DisplayAlert(Constants.authFail, $"{string.Join(Environment.NewLine, response.Errors)}", Constants.tryAgain);
                else
                    await DisplayAlert(Constants.authFail, $"{message}", Constants.tryAgain);

                return;
            }

            try
            {
                await SecureStorage.Default.SetAsync(Constants.jwt, response.Token);
                await SecureStorage.Default.SetAsync(Constants.refresh, response.RefreshToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                App.Token = response.Token;
            }

            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        }
    }
}

