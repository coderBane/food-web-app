using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace Foody.Admin.Templates;

public class CatagoryTable : ContentView
{
	public CatagoryTable()
	{
		Label id = new();
		Label name = new();
		Label status = new();
			
        id.SetBinding(Label.TextProperty, "Id");
        name.SetBinding(Label.TextProperty, "Name");
		status.SetBinding(Label.TextProperty,
			new Binding("IsActive", converter: new BoolObjectConverter
			{
				TrueObject = "Active",
				FalseObject = "Inactive"
			}));

		Ellipse circle = new()
        {
			HeightRequest = 20,
			WidthRequest = 20,
        };

		circle.SetBinding(Ellipse.FillProperty,
			new Binding("IsActive", converter: new BoolObjectConverter
            {
				TrueObject = Colors.LawnGreen,
				FalseObject = Colors.Red
            }));

		var hlayout = new HorizontalStackLayout
		{
			Spacing = 5,
			Children =
            {
				circle,
				status
            }
		};

        Grid grid = new()
        {
            ColumnDefinitions = { new(), new(), new() }
        };

        grid.Add(id);
		grid.Add(name, 1);
		grid.Add(hlayout, 2);

		var src = 
			new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext, typeof(ViewModels.Category.CategoryVM));

		var click = new TapGestureRecognizer();
		click.SetBinding(TapGestureRecognizer.CommandProperty, new Binding("TappedCommand", source: src));
		click.SetBinding(TapGestureRecognizer.CommandParameterProperty, ".");

		Frame mainFrame = new Frame
        {
			CornerRadius = 0,
            BorderColor = Colors.DarkGray,
            BackgroundColor = Colors.LightGray,
			Content = grid
        };

		mainFrame.GestureRecognizers.Add(click);

		Content = mainFrame;
	}
}

