using System.Collections;
using System.Windows.Input;

namespace Foody.Admin.Control;

public partial class TableView : ContentView
{
	public static readonly BindableProperty HasSearchBarProperty =
		BindableProperty.Create(nameof(HasSearchbar), typeof(bool), typeof(TableView), false);

	public static readonly BindableProperty ColumnNamesProperty =
		BindableProperty.Create(nameof(ColumnNames), typeof(string[]), typeof(TableView), Array.Empty<string>());

	public static readonly BindableProperty CollectionProperty =
		BindableProperty.Create(nameof(Collection), typeof(IEnumerable), typeof(TableView), Enumerable.Empty<object>(),
			propertyChanged: (bindable, oldValue, newValue) => { });

    public static readonly BindableProperty TemplateProperty =
		BindableProperty.Create(nameof(Template), typeof(DataTemplate), typeof(TableView), default(DataTemplate));

	public static readonly BindableProperty CreateCommandProperty =
		BindableProperty.Create(nameof(CreateCommand), typeof(ICommand), typeof(TableView));

    public static readonly BindableProperty ReloadCommandProperty =
        BindableProperty.Create(nameof(ReloadCommand), typeof(ICommand), typeof(TableView));

	public static readonly BindableProperty IsReloadEnabledProperty =
		BindableProperty.Create(nameof(IsReloadEnabled), typeof(bool), typeof(TableView), true,
			propertyChanged: (bindable, odlValue, newValue) => { });

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

	public ICommand CreateCommand
	{
        get => (ICommand)GetValue(CreateCommandProperty);
        set => SetValue(CreateCommandProperty, value);
    }

    public ICommand ReloadCommand
    {
        get => (ICommand)GetValue(ReloadCommandProperty);
        set => SetValue(ReloadCommandProperty, value);
    }

	public bool IsReloadEnabled
    {
		get => (bool)GetValue(IsReloadEnabledProperty);
		set => SetValue(IsReloadEnabledProperty, value);
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
					columnNames.Add(new Label{ Text = name }, count++);
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

