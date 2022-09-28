using Foody.Admin.Utilities;
using Foody.Admin.ViewModels.Category;
using Image = Microsoft.Maui.Controls.Image;

namespace Foody.Admin.Pages.Category;

public partial class ModifyPage : ContentPage
{
	public ModifyPage(CatogoryModifyVM viewmodel)
	{
		InitializeComponent();

		BindingContext = viewmodel;          
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    async void Upload_Clicked(Object sender, System.EventArgs e)
    {
        var file = await FilePicker.Default.PickAsync();

        if (file is not null)
        {
            var stream = await file.OpenReadAsync();
            var img = ImageSource.FromStream(() => stream);
            iname.Text = file.FileName;
            image.Source = img;
            
        }
    }
}

