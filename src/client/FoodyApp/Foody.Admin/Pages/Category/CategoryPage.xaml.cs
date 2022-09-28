using Foody.Admin.ViewModels.Category;

namespace Foody.Admin.Pages.Category;

public partial class CategoryPage : ContentPage
{
	public CategoryPage(CategoryVM category)
	{
		InitializeComponent();

		BindingContext = category;
	}

    async void Create_Button(Object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("categorymod", true,
			new Dictionary<string, object>()
            {
                ["Category"] = new Models.Category(),
                ["IsNew"] = true
            });
    }
}
