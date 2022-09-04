namespace Foody.Admin.Pages;

public partial class CategoryPage : ContentPage
{
	public CategoryPage(CategoryVM category)
	{
		InitializeComponent();

		BindingContext = category;
	}
}
