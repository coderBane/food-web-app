namespace Foody.Admin.Control;

public class FooterView : ContentView
{
	public FooterView()
	{
		Grid grid = new()
		{
			ColumnDefinitions = new()
            {
				new ColumnDefinition(),
				new ColumnDefinition(),
            }
		};

		grid.Add(new Label
		{
			Text = "Copyright \xa9\ufe0f foody.admin 2022",
			TextColor = Colors.Gray
		});

		Content = new Frame
		{
			CornerRadius = 0,
			BackgroundColor = Colors.White,
			BorderColor = Colors.LightGray,
			VerticalOptions = LayoutOptions.Center,
			HeightRequest = 65,

			Content = grid
		};
	}
}
