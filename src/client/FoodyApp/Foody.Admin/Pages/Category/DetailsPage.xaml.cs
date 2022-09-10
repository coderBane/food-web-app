using Foody.Admin.ViewModels.Category;

namespace Foody.Admin.Pages.Category;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(CategoryDetailsVM viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}

