using Foody.Admin.ViewModels.Category;

namespace Foody.Admin.Pages.Category;

public partial class CategoryPage : ContentPage
{
	public CategoryPage(CategoryVM category)
	{
		InitializeComponent();

		BindingContext = category;
	}
}
