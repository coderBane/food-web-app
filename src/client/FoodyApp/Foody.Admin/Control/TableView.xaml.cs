using System.Collections;

namespace Foody.Admin.Control;

public partial class TableView : ContentView
{
	public ObservableCollection<object> Item { get; } = new();

	public static readonly BindableProperty HasSearchBarProperty =
		BindableProperty.Create(nameof(HasSearchbar), typeof(bool), typeof(TableView), false);

	public static readonly BindableProperty ColumnNamesProperty =
		BindableProperty.Create(nameof(ColumnNames), typeof(string[]), typeof(TableView), Array.Empty<string>());

	public static readonly BindableProperty CollectionProperty =
		BindableProperty.Create(nameof(Collection), typeof(IEnumerable), typeof(TableView), Enumerable.Empty<object>(),
			propertyChanged: (bindable, oldValue, newValue) => { });

    public static readonly BindableProperty TemplateProperty =
		BindableProperty.Create(nameof(Template), typeof(DataTemplate), typeof(TableView), default(DataTemplate));

	public TableView()
	{
		InitializeComponent();
	}

	public bool HasSearchbar
	{
		get => (bool)GetValue(HasSearchBarProperty);
		set => SetValue(HasSearchBarProperty, value);
	}

	public IEnumerable Collection
	{
		get => (IEnumerable)GetValue(CollectionProperty);
		set => SetValue(CollectionProperty, value);
	}

	public DataTemplate Template
	{
		get => (DataTemplate)GetValue(TemplateProperty);
		set => SetValue(TemplateProperty, value);
	}

	public string[] ColumnNames
    {
		get => (string[])GetValue(ColumnNamesProperty);
		set => SetValue(ColumnNamesProperty, value);
    }

	public ColumnDefinitionCollection Columns
	{
        get
        {
            int count = 0;
            ColumnDefinitionCollection definitions = new();
			if (ColumnNames.Any())
				foreach (var name in ColumnNames)
                {
                    definitions.Add(new());
					columnNames.Add(new Label
					{
						Text = name,
						TextColor = Colors.Black
					}, count++);
                }

			return definitions;
        }
	}

  //  private static void CollectionChangedProperty(BindableObject bindable, object oldValue, object newValue)
  //  {
		//var control = (TableView)bindable;
		//control.Collection = (IEnumerable)newValue;
  //  }
}

