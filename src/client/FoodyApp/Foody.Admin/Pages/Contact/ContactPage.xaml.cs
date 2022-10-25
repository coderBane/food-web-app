using Foody.Admin.ViewModels.Contact;

namespace Foody.Admin.Pages.Contact;

public partial class ContactPage : ContentPage
{
	public ContactPage(ContactVM viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}
